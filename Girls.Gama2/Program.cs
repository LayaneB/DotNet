
using Girls.Gama2.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Girls.Gama2
{
    class Program
    {
        private static bool consultar = true;
        private static List<Dinheiro> listaComprasEmDinheiro;
        private static List<Boleto> listaBoletos;
        static void Main(string[] args)
        {
            listaBoletos = new List<Boleto>();
            listaComprasEmDinheiro = new List<Dinheiro>();

            while (Program.consultar)
            {
                Console.WriteLine("============================ Loja Garota Geek ================================");
                Console.WriteLine("Selecione uma das opções abaixo:");
                Console.WriteLine("1-Compra | 2-Pagamento | 3-Relatório | 4-Encerrar Consulta");

                var opcao = int.Parse(Console.ReadLine());

                switch (opcao)
                {
                    case 1:
                        Comprar();
                        break;
                    case 2:
                        Pagamento();
                        break;
                    case 3:
                        Relatorio();
                        break;
                    case 4:
                        Program.consultar = false;
                        break;
                    default:
                        break;
                }
            }
        }

        public static void Comprar()
        {

            Console.WriteLine("==================================== Compra =====================================");
            Console.WriteLine("Deseja gerar boleto?");
            Console.WriteLine("1-Sim | 2-Não");
            Console.WriteLine("==================================== Compra =====================================");

            var tipoPagamento = int.Parse(Console.ReadLine());

            switch (tipoPagamento)
            {
                case 1:
                    ViaBoleto();
                    break;
                case 2:
                    EmDinheiro();
                    break;
                default:
                    break;
            }

        }

        public static void EmDinheiro()
        {
            Console.WriteLine("Digite o valor da compra:");
            var valor = double.Parse(Console.ReadLine());

            Console.WriteLine("Digite o CPF do cliente:");
            var cpf = Console.ReadLine();

            Console.WriteLine("Preeencha a descrição do produto");
            var Produto = Console.ReadLine();

            var dinheiro = new Dinheiro(cpf, valor, Produto);
            dinheiro.CriarCompra();

            Console.WriteLine("============================== Compra em Dinheiro ===============================");
            Console.WriteLine($"A compra do produto: {Produto} foi realizada com sucesso!");
            Console.WriteLine("Realizar o pagamento em dinheiro dentro dos próximos 30 dias.");
            Console.WriteLine("============================== Compra em Dinheiro ===============================");

            listaComprasEmDinheiro.Add(dinheiro);
        }

        public static void ViaBoleto()
        {
            Console.WriteLine("Digite o valor da compra:");
            var valor = double.Parse(Console.ReadLine());

            Console.WriteLine("Digite o CPF do cliente:");
            var cpf = Console.ReadLine();

            Console.WriteLine("Preeencha uma descrição do produto");

            var descricao = Console.ReadLine();
            var boleto = new Boleto(cpf, valor, descricao);
            boleto.GerarBoleto();

            Console.WriteLine($"Boleto gerado com sucesso de número {boleto.CodigoBarra} com data de vencimento para o dia {boleto.DataVencimento} ");

            listaBoletos.Add(boleto);
        }

        public static void Pagamento()
        {

            Console.WriteLine("=================================== Pagamento ===================================");
            Console.WriteLine("Como deseja realizar o pagamento de sua compra:");
            Console.WriteLine("1-Boleto | 2-Dinheiro");
            Console.WriteLine("=================================== Pagamento ===================================");

            var tipoPagamento = int.Parse(Console.ReadLine());

            switch (tipoPagamento)
            {
                case 1:
                    PagamentoBoleto();
                    break;
                case 2:
                    PagamentoDinheiro();
                    break;
                default:
                    break;
            }
        }

        public static void PagamentoBoleto()
        {         
            Console.WriteLine("========================== Pagamento via Boleto =================================");
            Console.WriteLine("Digite o código de barras:");
            var numero = Guid.Parse(Console.ReadLine());

            var boleto = listaBoletos
                            .Where(item => item.CodigoBarra == numero)
                            .FirstOrDefault();

            if (boleto is null)
            {
                Console.WriteLine($"Boleto de código {numero} não encontrado!");
                return;
            }

            if (boleto.EstaPago())
            {
                Console.WriteLine($"Boleto foi pago no dia {boleto.DataPagamento}");
                return;
            }

            if (boleto.EstaVencido())
            {
                boleto.CalcularJuros();
                Console.WriteLine($"Boleto está vencido, terá acrescimo de 10% === R$ {boleto.Valor}");
            }

            boleto.Pagar();
            Console.WriteLine($"Boleto de código {numero} foi pago com sucesso");
            Console.WriteLine("========================== Pagamento via Boleto =================================");
        }
        public static void PagamentoDinheiro()
        {
            Console.WriteLine("========================== Pagamento em Dinheiro ================================");

            Console.WriteLine("Insira o valor da compra:");
            var Valor= Console.ReadLine();

            Console.WriteLine("Digite a descrição do produto:");
            var Produto = Console.ReadLine();

            var dinheiro = listaComprasEmDinheiro
                            .Where(item => item.Produto == Produto)
                            .FirstOrDefault();

            if (dinheiro is null)
            {
                Console.WriteLine($"A compra do produto {Produto} não foi encontrada no sistema!");
                return;
            }

            if (dinheiro.CompraPaga())
            {
                Console.WriteLine($"O pagamento deste produto foi realizado no dia {dinheiro.DataPagamentoDinheiro}");
                return;
            }

            if (dinheiro.ContaVencida())
            {
                dinheiro.CalcularJuros();
                Console.WriteLine($"A conta está vencida, terá acrescimo de 10% === R$ {dinheiro.ValorDinheiro}");
            }

            dinheiro.Pagar();
            Console.WriteLine($"O produto {Produto} de valor {Valor} foi pago com sucesso");


            Console.WriteLine("========================== Pagamento em Dinheiro ================================");

        }

        public static void Relatorio()
        {
            Console.WriteLine("================================= Relatório =====================================");
            Console.WriteLine("Qual você deseja acessar o relatório de compras cujos pagamentos foram:");
            Console.WriteLine("1-Via Boleto | 2-Em dinheiro | 3-Ambos");
            Console.WriteLine("================================= Relatório =====================================");

            var opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    RelatorioBoleto();
                    break;
                case 2:
                    RelatorioDinheiro();
                    break;
                case 3:
                    RelatorioAmbos();
                    break;
                default:
                    break;
            }
        }
        public static void RelatorioBoleto()
        {
            Console.WriteLine("=========================== Relatório- via Boleto ===============================");
            Console.WriteLine("Você deseja acessar o relatório referente à boletos:");
            Console.WriteLine("1-Pagos | 2-À pagar | 3-Vencidos");
            Console.WriteLine("=========================== Relatório- via Boleto ===============================");

            var opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    BoletosPagos();
                    break;
                case 2:
                    BoletosAPagar();
                    break;
                case 3:
                    BoletosVencidos();
                    break;
                default:
                    break;
            }
        }
        public static void RelatorioDinheiro()
        {
            Console.WriteLine("========================== Relatório- via Dinheiro ==============================");
            Console.WriteLine("Você deseja acessar o relatório referente à contas:");
            Console.WriteLine("1-Pagas | 2-À pagar | 3-Vencidas");
            Console.WriteLine("========================== Relatório- via Dinheiro ==============================");

            var opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    ContasPagas();
                    break;
                case 2:
                    ContasAPagar();
                    break;
                case 3:
                    ContasVencidos();
                    break;
                default:
                    break;
            }
        }

        public static void RelatorioAmbos()
        {
            Console.WriteLine("============================== Relatório geral ==================================");
            Console.WriteLine("Você deseja acessar o relatório referente à compras:");
            Console.WriteLine("1-Pagas | 2-À pagar | 3-Vencidas");
            Console.WriteLine("============================== Relatório geral ==================================");

            var opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    ContasPagas();
                    BoletosPagos();
                    break;
                case 2:
                    ContasAPagar();
                    BoletosAPagar();
                    break;
                case 3:
                    ContasVencidos();
                    BoletosVencidos();
                    break;
                default:
                    break;
            }
        }

        public static void BoletosPagos()
        {
            Console.WriteLine("=============================== Boletos Pagos ===================================");
            var boletos = listaBoletos
                            .Where(item => item.Confirmacao)
                            .ToList();

            foreach (var item in boletos)
            {
                Console.WriteLine("\n ====");
                Console.WriteLine($"Codigo de Barra: {item.CodigoBarra}\nValor:{item.Valor}\nData Pagamento: {item.DataPagamento} ==");
            }

            Console.WriteLine("=============================== Boletos Pagos ===================================\n");
        }
        public static void ContasPagas()
        {
            Console.WriteLine("================================ Contas Pagas ===================================");
            var dinheiro = listaComprasEmDinheiro
                            .Where(item => item.ConfirmacaoPagamento)
                            .ToList();

            foreach (var item in dinheiro)
            {
                Console.WriteLine("\n ====");
                Console.WriteLine($"Produto: {item.Produto}\nValor:{item.ValorDinheiro}\nData Pagamento: {item.DataPagamentoDinheiro} ==");
            }

            Console.WriteLine("================================ Contas Pagas ===================================\n");
        }

        public static void BoletosAPagar()
        {
            Console.WriteLine("============================== Boletos à Pagar ==================================");
            var boletos = listaBoletos
                            .Where(item => item.Confirmacao == false
                                    && item.DataVencimento > DateTime.Now)
                            .ToList();

            foreach (var item in boletos)
            {
                Console.WriteLine("\n ====");
                Console.WriteLine($"Codigo de Barra: {item.CodigoBarra}\nValor:{item.Valor}\nData Pagamento: {item.DataPagamento} ==");
            }

            Console.WriteLine("============================== Boletos à Pagar ==================================\n");
        }

        public static void ContasAPagar()
        {
            Console.WriteLine("=============================== Contas à Pagar ==================================");

            var dinheiro = listaComprasEmDinheiro
                            .Where(item => item.ConfirmacaoPagamento == false
                                    && item.DataVencimento > DateTime.Now)
                            .ToList();

            foreach (var item in dinheiro)
            {
                Console.WriteLine("\n ====");
                Console.WriteLine($"Produto: {item.Produto}\nValor:{item.ValorDinheiro}\nData Pagamento: {item.DataPagamentoDinheiro} ==");
            }

            Console.WriteLine("=============================== Contas à Pagar ==================================\n");
        }

        public static void BoletosVencidos()
        {
            Console.WriteLine("============================== Boletos Vencidos =================================");
            var boletos = listaBoletos
                            .Where(item => item.Confirmacao == false
                                    && item.DataVencimento < DateTime.Now)
                            .ToList();

            foreach (var item in boletos)
            {
                Console.WriteLine("\n ====");
                Console.WriteLine($"Codigo de Barra: {item.CodigoBarra}\nValor:{item.Valor}\nData Pagamento: {item.DataPagamento} ==");
            }

            Console.WriteLine("============================== Boletos Vencidos =================================\n");
        }

        public static void ContasVencidos()
        {
            Console.WriteLine("=============================== Contas Vencidos =================================");
            var dinheiro = listaComprasEmDinheiro
                            .Where(item => item.ConfirmacaoPagamento == false
                                    && item.DataVencimento < DateTime.Now)
                            .ToList();

            foreach (var item in dinheiro)
            {
                Console.WriteLine("\n ====");
                Console.WriteLine($"Produto {item.Produto}\nValor:{item.ValorDinheiro}\nData Pagamento: {item.DataPagamentoDinheiro} ==");
            }

            Console.WriteLine("=============================== Contas Vencidos =================================\n");
        }
    }
}

