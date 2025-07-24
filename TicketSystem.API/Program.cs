using Microsoft.EntityFrameworkCore;
using TicketSystem.Core.Interfaces;
using TicketSystem.Infrastructure.Data;
using TicketSystem.Infrastructure.Data.Repositories;
using TicketSystem.Application.Services; 
using TicketSystem.Application.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// DbContext'i servislere ekle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// Repository'leri baðýmlýlýk enjeksiyonu için kaydet
// AddScoped kullanýldý. Her HTTP isteði için yeni bir instance oluþturur.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();

//Uygulama Servislerini baðýmlýlýk için kaydetme
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();


// Diðer servis eklemeleriniz...
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// CORS politikasý
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder.WithOrigins("http://localhost:4200") // Angular uygulamasýnýn adresi
                           .AllowAnyHeader()
                           .AllowAnyMethod());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthorization();
app.MapControllers();
app.Run();