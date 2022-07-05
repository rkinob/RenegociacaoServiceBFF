namespace RenegociacaoServiceBFF.Models;

public class Oferta
{
    public string idOferta { get; set; }
    public double valor { get; set; }
    public double valorOferta { get; set; }
    public DateTime dataVencimento { get; set; }
    public string idCliente { get; set; }
    public List<Contrato> contratos { get; set; }
}