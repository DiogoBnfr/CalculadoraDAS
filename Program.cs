using System.Globalization;
public class Program
{
    public static void Main()
    {
        CultureInfo culture = new CultureInfo("pt-BR");

        string input;
        decimal resultado = 0;

        // Fórmula do cálculo DAS:
        // (faturamento últimos 12 meses * alíquota Simples Nacional da tabela) – dedução da tabela

        Console.Write("Insira o faturamento anual da empresa: ");
        input = Console.ReadLine().ToString(culture);
        if (string.IsNullOrEmpty(input)) input = "0";
        decimal faturamento_anual = Convert.ToDecimal(input, culture);

        Console.Write("Insira o percentual da alíquota do Simples Nacional: ");
        input = Console.ReadLine().ToString(culture);
        if (string.IsNullOrEmpty(input)) input = "0";
        decimal percentual_alíquota = Convert.ToDecimal(input, culture);

        Console.Write("Insira a dedução da tabela do Simples Nacional: ");
        // warning CS8602: Dereference of a possibly null reference.
        input = Console.ReadLine().ToString(culture);
        if (string.IsNullOrEmpty(input)) input = "0";
        decimal dedução_tabela = Convert.ToDecimal(input, culture);

        decimal faturamento_mensal = Convert.ToDecimal(faturamento_anual / 12, culture);

        try
        {
            resultado = faturamento_anual * (percentual_alíquota / 100);
            resultado = resultado - dedução_tabela;
            resultado = resultado / faturamento_anual;
            resultado = faturamento_mensal * resultado;
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("Erro: Tentativa de divisão por zero. Por favor verifique se os valores inseridos estão corretos.");
        }
        finally
        {
            Console.WriteLine("Cálculo da guia do DAS (Documento de Arrecadação do Simples Nacional)");
            Console.WriteLine($"Emitido em: {DateTime.Now}");
            Console.WriteLine($"Faturamento Anual informado: {faturamento_anual.ToString("N2")}");
            Console.WriteLine($"Faturamento Mensal estimado: {faturamento_mensal.ToString("N2")}");
            Console.WriteLine($"Percentual de Alíquota informado: {percentual_alíquota.ToString("N2")}");
            Console.WriteLine($"Dedução de pagamento informada: {dedução_tabela.ToString("N2")}");
            Console.WriteLine($"Valor Final da Guia: {resultado.ToString("N2")}");
        }
    }
}