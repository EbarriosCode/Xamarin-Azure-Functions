# Xamarin-Azure-Functions

**Código Azure Function**

#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function procesando petición.");    

    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);    
    
    if(data != null)
    {
        double peso = Convert.ToDouble(data.peso);
        double altura = Convert.ToDouble(data.altura);
        double result = (peso/(altura * altura));        
        
        return (ActionResult)new OkObjectResult(new { Result = Math.Truncate(result)});
    }
    else
        return new BadRequestObjectResult("Por favor envie un nombre, altura en metros y peso en kilogramos en el cuerpo de la solicitud");     
}
