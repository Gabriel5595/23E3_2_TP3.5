using System;

public abstract class Conta
{
    public double SaldoAtualEmReais { get; protected set; }

    public Conta(double saldoAtualEmReais)
    {
        SaldoAtualEmReais = saldoAtualEmReais;
    }
}

// Crie a interface ITarifa
public interface ITarifa
{
    double Calcular();
}

public class ContaCorrente : Conta, ITarifa
{
    public ContaCorrente(double saldoAtualEmReais) : base(saldoAtualEmReais) { }

    public double Calcular()
    {
        return SaldoAtualEmReais * 0.015;
    }
}

public class ContaInternacional : Conta, ITarifa
{
    public double TaxaCambio { get; private set; }

    public ContaInternacional(double saldoAtualEmDolares, double taxaCambio) : base(saldoAtualEmDolares)
    {
        TaxaCambio = taxaCambio;
        SaldoAtualEmReais = saldoAtualEmDolares * taxaCambio;
    }

    public double Calcular()
    {
        return SaldoAtualEmReais * 0.025;
    }
}

public class ContaCripto : Conta
{
    public ContaCripto(double saldoAtualEmDolares) : base(saldoAtualEmDolares) { }
}

public class SistemaTarifas
{
    public double ValorTotalSaldo { get; private set; }
    public double ValorTotalTarifa { get; private set; }

    public void AdicionarConta(Conta conta)
    {
        ValorTotalSaldo += conta.SaldoAtualEmReais;

        if (conta is ITarifa contaComTarifa)
        {
            ValorTotalTarifa += contaComTarifa.Calcular();
        }
    }
}

class Program
{
    static void Main()
    {
        SistemaTarifas sistema = new SistemaTarifas();

        while (true)
        {
            Console.WriteLine("Escolha o tipo de conta (1 - Conta Corrente, 2 - Conta Internacional, 3 - Conta Cripto, 4 - Sair):");
            int escolha = int.Parse(Console.ReadLine());

            if (escolha == 4)
                break;

            Console.Write("Informe o saldo atual: ");
            double saldo = double.Parse(Console.ReadLine());

            switch (escolha)
            {
                case 1:
                    ContaCorrente contaCorrente = new ContaCorrente(saldo);
                    sistema.AdicionarConta(contaCorrente);
                    break;

                case 2:
                    Console.Write("Informe a taxa de câmbio (USD para BRL): ");
                    double taxaCambio = double.Parse(Console.ReadLine());
                    ContaInternacional contaInternacional = new ContaInternacional(saldo, taxaCambio);
                    sistema.AdicionarConta(contaInternacional);
                    break;

                case 3:
                    ContaCripto contaCripto = new ContaCripto(saldo);
                    sistema.AdicionarConta(contaCripto);
                    break;

                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }
        }

        Console.WriteLine($"Total do Saldo: {sistema.ValorTotalSaldo} reais");
        Console.WriteLine($"Total da Tarifa: {sistema.ValorTotalTarifa} reais");
    }
}