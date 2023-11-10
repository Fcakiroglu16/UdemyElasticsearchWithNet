//using Elasticsearch.Net;
//using Nest;
using ElasticSearch.API.Extensions;
using ElasticSearch.API.Services;
using ElasticSearch.API.Repository;
using ElasticSearch.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var pool = new SingleNodeConnectionPool(new Uri(builder.Configuration.GetSection("Elastic")["Url"]!));
//// ! işareti ile diyoruz ki bana kızma bu data kesinlikle var bana kızma diyoruz.
//var settings = new ConnectionSettings(pool);
//var client = new ElasticClient(settings);
//builder.Services.AddSingleton(client);
//Şimdi Program.cs i kirletmemek adına Extensions(Uzantılar) isimli bir klasör ve içerisinde bir class oluşturup bu 13-17 
//arası kodları oraya taşıdık.
//Sonrasında o sınıfın namespaceini aldık yukarı yazdık ve şimdi aşağıya ekleyeceğiz.

builder.Services.AddElasticService(builder.Configuration);

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductRepository>();

builder.Services.AddScoped<ECommerceRepository>();


//Uygulamanızın genelinde kullandığınız tüm sınıflarınızı kesinlikle new lemeyin.
//Nerden alın diğer Container'dan alın.Bunlar Repository'ler, Servisleriniz, Helperlarınız olabilir.
//Bunlar uygulamanızın genelinde kullanabileceğiniz sınıflardır.
//Bu sınıfları diğer Container'dan alırsak servisle beraber AddScope,AddTransaint,AddSingleton
//yaşam döngüsüne müdahalade bulunabileceğimiz bazı metodlar geliyor.Bırakalım o sınıfın yaşam döngüsünü Container halletsin
//Örneğin builder üzerinden bir AddScope eklersek bu şu anlama gelir burdaki sınıf her bir istek geldiğinde üretilir Responsela döndüğü anda da o sınıf memoryden düşer.

//AddScoped(): Her bir istekte bir nesne örneği üretilir.Response döndüğü anda o nesne yıkılsın.
//AddTransient(): Her çağırdığımızda bir nesne örneği üretilir.
//AddSingleton(): Uygulama boyunca tek bir nesne örneği üretilir.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
