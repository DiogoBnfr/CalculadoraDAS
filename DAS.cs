using System.ComponentModel;

namespace CalculadoraDAS;

public class DAS
{
    [DisplayName("Data de Emissão")]
    public DateTime data_emissão { get; set; } = DateTime.Now;
    [DisplayName("Faturamento Anual")]
    public decimal faturamento_anual { get; set; }
    [DisplayName("Faturamento Mensal")]
    public decimal faturamento_mensal { get; set; }
    [DisplayName("Alíquota da Faixa")]
    public decimal percentual_alíquota { get; set; }
    [DisplayName("Dedução de Pagamento")]
    public decimal dedução_tabela { get; set; }
    [DisplayName("Valor Final")]
    public decimal valor_final { get; set; }

    public DAS(DateTime data_emissão, decimal faturamento_anual, decimal faturamento_mensal,
    decimal percentual_alíquota, decimal dedução_tabela, decimal valor_final)
    {
        this.data_emissão = data_emissão;
        this.faturamento_anual = faturamento_anual;
        this.faturamento_mensal = faturamento_mensal;
        this.percentual_alíquota = percentual_alíquota;
        this.dedução_tabela = dedução_tabela;
        this.valor_final = valor_final;
    }
}