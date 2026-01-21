using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;
using WebApplication1.ClientApp.DTOs;
using Microsoft.Data.SqlClient;


namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrijsGeschiedenisController : ControllerBase
    {
        //private readonly string _connectionString;

        //public PrijsGeschiedenisController(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetConnectionString("DefaultConnection")
        //        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        //}

        //[HttpGet("id/{id}")]
        //public async Task<ActionResult<PlantPrijsSamenvattingDto>> GetPrijsGeschiedenis(int id)
        //{

        //    string soortPlant;
        //    string aanvoerderNaam;

        //    using var connection = new SqlConnection(_connectionString);
        //    await connection.OpenAsync();

        //    // QUERY 1: soortPlant
        //    var soortCommand = new SqlCommand("""
        //        SELECT soortPlant
        //        FROM Product
        //        WHERE productId = @productId
        //    """, connection);

        //    soortCommand.Parameters.AddWithValue("@productId", id);

        //    var soortResult = await soortCommand.ExecuteScalarAsync();
        //    if (soortResult == null)
        //        return NotFound();

        //    soortPlant = (string)soortResult;

        //    // QUERY 2: aanvoerderNaam
        //    var aanvoerderCommand = new SqlCommand("""
        //        SELECT aanvoerderNaam
        //        FROM Product
        //        WHERE productId = @productId
        //    """, connection);

        //    aanvoerderCommand.Parameters.AddWithValue("@productId", id);

        //    var aanvoerderResult = await aanvoerderCommand.ExecuteScalarAsync();
        //    if (aanvoerderResult == null)
        //        return NotFound();

        //    aanvoerderNaam = (string)aanvoerderResult;

        //    var ppsdto = new PlantPrijsSamenvattingDto
        //    {
        //        soortPlant = soortPlant,
        //        aanvoerderNaam = aanvoerderNaam
        //    };

        //    return Ok(ppsdto);
        //}


    }
}