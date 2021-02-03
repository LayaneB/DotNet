using System;
using System.Collections.Generic;
using System.Text;

namespace Girls.Gama2.Entidades
{
    class Dinheiro
    {
        private const int DiasVencimento = 30;
        private const double Juros = 0.10;

        public Dinheiro(string cpf,
                        double valor,
                        string produto)
        {
            Cpf = cpf;
            ValorDinheiro = valor;
            Produto = produto;
            DataEmissao = DateTime.Now;
            ConfirmacaoPagamento = false;
        }

        public double ValorDinheiro { get; set; }
        public DateTime DataDaCompra { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataPagamentoDinheiro { get; set; }
        public bool ConfirmacaoPagamento { get; set; }
        public string Cpf { get; set; }
        public string Produto { get; set; }

        public void CriarCompra()
        {

            DataVencimento = DataEmissao.AddDays(DiasVencimento);
        }

        public bool CompraPaga()
        {
            return ConfirmacaoPagamento;
        }

        public bool ContaVencida()
        {
            return DataVencimento< DateTime.Now;
        }

        public void CalcularJuros()
        {
            var taxa = ValorDinheiro * Juros;
            ValorDinheiro += taxa;
        }

        public void Pagar()
        {
            DataPagamentoDinheiro = DateTime.Now;
            ConfirmacaoPagamento = true;
        }
    }
}
