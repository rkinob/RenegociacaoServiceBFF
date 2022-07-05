namespace RenegociacaoServiceBFF.Models;

public class Contrato
{
   public string idContrato { get; set; }
   public int idAgrupamento { get; set; }
   public string numero { get; set; }
   public string descricao { get; set; }
   public double valor { get; set; }
   public DateTime dataVencimento { get; set; }
   public string idCliente { get; set; }
   public Cliente cliente { get; set; }
 }