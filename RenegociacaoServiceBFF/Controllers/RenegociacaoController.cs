using Microsoft.AspNetCore.Mvc;
using RenegociacaoServiceBFF.Models;

namespace RenegociacaoServiceBFF.Controllers;

[ApiController]
public class RenegociacaoController : ControllerBase
{
    // prop for cliente
    public Cliente clientePadrao = new Cliente {
            nome = "Cliente Sobrenome",
            cpf = "51313961078",
            email = "teste@teste.com.br",
            idCliente = "5239a7bc-c542-43e7-bc17-78228d9e63a0"
    };

    [HttpGet]
    [Route("api/renegociacao/ConsultarCliente/{cpf}")]
    public IActionResult ConsultarCliente(string cpf)
    {
        if(string.IsNullOrWhiteSpace(cpf)) 
            return BadRequest("CPF deve ser informado");

        if (clientePadrao.cpf != cpf)
            return BadRequest("CPF não encontrado");


        return Ok(this.clientePadrao);
    }
   
    [HttpPut]
    [Route("api/renegociacao/SalvarCliente")]
    public IActionResult SalvarCliente(Cliente cliente)
    {
        return Ok(cliente);
    }

    [HttpGet]
    [Route("api/renegociacao/ConsultarContratos/{cpf}")]
   
    public IActionResult ConsultarContratos(string cpf)
    {
        var contratos = Enumerable.Range(1, 4).Select(index => new Contrato
        {
            idContrato = Guid.NewGuid().ToString(),
            idAgrupamento = index%2 == 0 ? 1 : 2,
            numero = "Contrato " + index,
            descricao = "Descrição " + index,
            valor = GetRandomNumber(500,1500),
            dataVencimento = DateTime.Now.AddDays(- index * 2),
            idCliente = this.clientePadrao.idCliente,
            cliente = this.clientePadrao
        }); 
        return Ok(contratos);
    }

    [HttpPost]
    [Route("api/renegociacao/ConsultarOfertas")]
    public IActionResult ConsultarOfertas([FromBody] List<Contrato> contratos)
    {
        List<Contrato> agrupamentosDistintos = contratos.GroupBy(p => p.idAgrupamento)
                                                      .Select(g => g.First())
                                                      .ToList();
        List<Oferta> ofertas = new List<Oferta>();

        for(int i = 0; i < agrupamentosDistintos.Count; i++)
        {
            var contrato = agrupamentosDistintos[i];

            var somaContrato = contratos.Where(a => a.idAgrupamento == contrato.idAgrupamento).Sum(soma => soma.valor);
            var contratosdoGrupo = contratos.Where(a => a.idAgrupamento == contrato.idAgrupamento).ToList();
            ofertas.Add(new Oferta
            {
                idOferta = Guid.NewGuid().ToString(),
                valor = somaContrato,
                valorOferta = somaContrato - (somaContrato * 0.25),
                dataVencimento = DateTime.Now.AddDays(2),
                idCliente = this.clientePadrao.idCliente,
                contratos = contratosdoGrupo
            });
        }
       
        return Ok(ofertas);
        
    }

    [HttpPost]
    [Route("api/renegociacao/ConfirmarOfertas")]
    public IActionResult ConfirmarOfertas([FromBody] List<Oferta> ofertas)
    {
        return Ok(ofertas);
    }

    private double GetRandomNumber(double minimum, double maximum)
    {
        Random random = new Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }

}
