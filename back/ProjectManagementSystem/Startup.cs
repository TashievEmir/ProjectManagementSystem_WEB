using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystem
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            string con = "Server=UKUK-31;Database=ProjectMS;Trusted_Connection=True;";
            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // ��������, ����� �� �������������� �������� ��� ��������� ������
                            ValidateIssuer = true,
                            // ������, �������������� ��������
                            ValidIssuer = AuthOptions.ISSUER,

                            // ����� �� �������������� ����������� ������
                            ValidateAudience = true,
                            // ��������� ����������� ������
                            ValidAudience = AuthOptions.AUDIENCE,
                            // ����� �� �������������� ����� �������������
                            ValidateLifetime = true,

                            // ��������� ����� ������������
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            // ��������� ����� ������������
                            ValidateIssuerSigningKey = true,
                        };
                    });
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(con));
            services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMemoryCache();
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            services.AddTransient<ARegistrationService, RegistrationService>();
            services.AddTransient<IResetPassword, ResetPasswordService>();
            services.AddTransient<ISaveProjectService, SaveProjectService>();
            services.AddTransient<ISaveTaskService, SaveTaskService>();
            services.AddTransient<IProjectsDataService, ProjectsDataService>();
            services.AddTransient<ITasksDataService, TasksDataService>();
            services.AddTransient<IUsersDataService, UsersDataService>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
