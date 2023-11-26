
using IDM_API.Entities;
using IDM_API.Services.Jwt;
using IDM_API.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using IDM_API.Services.Schedule;
using IDM_API.Services.Approval;
using IDM_API.Services.Assignment;
using IDM_API.Services.Proposal;
using IDM_API.Services.TaskWorking;
using IDM_API.Services.File;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using IDM_API.Services.Notification;
using IDM_API.Services.Department;
using IDM_API.Services.Progress;
using IDM_API.Services.Menu;

namespace IDM_API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = builder.Configuration["JWT_Configuration:Issuer"],
						ValidAudience = builder.Configuration["JWT_Configuration:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT_Configuration:SecretKey"]!))
					};
				});

			builder.Services.AddDbContext<IDMContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			builder.Services.AddCors(options =>
			{
				options.AddDefaultPolicy(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
			});

			builder.Services.Configure<FormOptions>(options =>
			{
				options.ValueLengthLimit = int.MaxValue;
				options.MultipartBodyLengthLimit = int.MaxValue;
				options.MemoryBufferThreshold = int.MaxValue;
			});

			builder.Services.AddScoped<IJwtService, JwtService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IScheduleService, ScheduleService>();
			builder.Services.AddScoped<IApprovalService,  ApprovalService>();
			builder.Services.AddScoped<IAssignmentService, AssignmentService>();
			builder.Services.AddScoped<IProposalService, ProposalService>();
			builder.Services.AddScoped<ITaskService, TaskSerivce>();
			builder.Services.AddScoped<IFileService, FileService>();
			builder.Services.AddScoped<IDepartmentService, DepartmentService>();
			builder.Services.AddScoped<IProgressService, ProgressService>();
			builder.Services.AddScoped<IMenuService, MenuService>();

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "IT Department Management API",
					Version = "v1"
				});
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new string[] {}
					}
				});
			});

			builder.Services.AddSignalR();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			//if (app.Environment.IsDevelopment())
			//{
			//	app.UseSwagger();
			//	app.UseSwaggerUI();
			//}
			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseHttpsRedirection();
			app.UseCors();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseStaticFiles();

			//app.MapHub<NotificationService>("/notification");
			app.MapControllers();

			app.Run();
		}
	}
}