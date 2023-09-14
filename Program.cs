using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace CalculadoraDAS;
internal class Program
{
    public static void Main()
    {
        CultureInfo culture = new CultureInfo("pt-BR");

        string? input;

        decimal faturamento_anual = 0;
        decimal faturamento_mensal = 0;
        decimal percentual_alíquota;
        decimal dedução_tabela;

        // Fórmula do cálculo DAS:
        // (faturamento últimos 12 meses * alíquota Simples Nacional da tabela) – dedução da tabela
        Console.Write("Insira a idade do CNPJ (maior que 12 meses, deixar em branco)?");
        input = Console.ReadLine()?.ToString(culture);
        if (string.IsNullOrEmpty(input)) input = "13";
        int mes = Convert.ToInt32(input, culture);

        if (mes > 0 && mes < 3)
        {
            Console.Write("Insira o faturamento do 1o mês da empresa: ");
            input = Console.ReadLine()?.ToString(culture);
            if (string.IsNullOrEmpty(input)) input = "0";
            faturamento_mensal = Convert.ToDecimal(input, culture);

            faturamento_anual = Convert.ToDecimal(faturamento_mensal * 12, culture);
        }
        if (mes > 2 && mes < 13)
        {

            Console.Write($"Insira o faturamento do {mes - 2}o mês da empresa: ");
            input = Console.ReadLine()?.ToString(culture);
            if (string.IsNullOrEmpty(input)) input = "0";
            decimal faturamento_mensal1 = Convert.ToDecimal(input, culture);

            Console.Write($"Insira o faturamento do {mes - 1}o mês da empresa: ");
            input = Console.ReadLine()?.ToString(culture);
            if (string.IsNullOrEmpty(input)) input = "0";
            decimal faturamento_mensal2 = Convert.ToDecimal(input, culture);

            faturamento_mensal = Convert.ToDecimal(faturamento_mensal1 + faturamento_mensal2 / 2, culture);

            faturamento_anual = Convert.ToDecimal(faturamento_mensal * 12);
        }
        if (mes > 12)
        {
            Console.Write("Insira o faturamento anual da empresa: ");
            input = Console.ReadLine()?.ToString(culture);
            if (string.IsNullOrEmpty(input)) input = "0";
            faturamento_anual = Convert.ToDecimal(input, culture);

            faturamento_mensal = Convert.ToDecimal(faturamento_anual / 12, culture);
        }
        Console.Write("Insira o percentual da alíquota do Simples Nacional: ");
        input = Console.ReadLine()?.ToString(culture);
        if (string.IsNullOrEmpty(input)) input = "0";
        percentual_alíquota = Convert.ToDecimal(input, culture);

        Console.Write("Insira a dedução da tabela do Simples Nacional: ");
        // warning CS8602: Dereference of a possibly null reference.
        input = Console.ReadLine()?.ToString(culture);
        if (string.IsNullOrEmpty(input)) input = "0";
        dedução_tabela = Convert.ToDecimal(input, culture);

        try
        {
            DAS _DAS = GerarDAS(faturamento_anual, faturamento_mensal, dedução_tabela, percentual_alíquota, mes);
            GerarExtratoDAS(_DAS);
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("Erro: Tentativa de divisão por zero. Por favor verifique se os valores inseridos estão corretos.");
        }
    }

    private static DAS GerarDAS(decimal faturamento_anual, decimal faturamento_mensal, decimal dedução_tabela, decimal percentual_alíquota, int? mes = 0)
    {
        decimal valor_final;
        valor_final = faturamento_anual * (percentual_alíquota / 100);
        valor_final = valor_final - dedução_tabela;
        valor_final = valor_final / faturamento_anual;
        valor_final = faturamento_mensal * valor_final;
        DateTime date_emissão = DateTime.Now;
        return new DAS(date_emissão, faturamento_anual, faturamento_mensal, percentual_alíquota, dedução_tabela, valor_final);
    }

    private static void GerarExtratoDAS(DAS _DAS)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Cálculo da guia do DAS (Documento de Arrecadação do Simples Nacional)");
        Console.ResetColor();

        Type type = _DAS.GetType();
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttribute != null)
            {
                string displayName = displayNameAttribute.DisplayName.PadRight(30, '.');
                object propertyValue = property.GetValue(_DAS) ?? "DisplayNameAttributeNotFound";

                Console.Write($"{displayName}");

                if (propertyValue.GetType() == typeof(decimal))
                    Console.WriteLine($"R${propertyValue:N2}");
                else
                    Console.WriteLine($"{propertyValue}");
            }
        }
    }
}