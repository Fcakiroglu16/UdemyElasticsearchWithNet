using System.Net;

namespace ElasticSearch.API.DTOs
{
    public record ResponseDTO<T>
    {
        public T? Data { get; set; }
        public HttpStatusCode StatusCode{ get; set; }
        public HashSet<String>? Errors { get; set; } 


        //Staric Factory Metod => Factory Method Design Pattern
        public static ResponseDTO<T> Succes(T data, HttpStatusCode statusCode)
        {
            return new ResponseDTO<T> { Data = data, StatusCode = statusCode };
            //Eğer başarılı ise datayı dolduracak ve başarılı status kodu dönecek.başarılı olduğunda errors doldurmayacağı için data ?(nullable)
        }


        public static ResponseDTO<T> Fail(HashSet<String> errors, HttpStatusCode statusCode)
        {
            return new ResponseDTO<T> { Errors = errors, StatusCode = statusCode };
            //Eğer başarısız  ise errors'u dolduracak ve başarısız status kodu dönecek.başarısız olduğunda datayı doldurmayacağı için data ?(nullable)
        }


        public static ResponseDTO<T> Fail(string errors, HttpStatusCode statusCode)
        {
            return new ResponseDTO<T> { Errors = new HashSet<string> { errors }, StatusCode = statusCode };
        }
    }


    
}
