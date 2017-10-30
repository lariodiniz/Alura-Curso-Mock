using mock.dominio;
using mock.servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using mock.infra;
using Moq;

namespace mock.testes
{
    [TestFixture]
    class EncerradorDeLeilaoTest
    {
        [Test]
        public void DeveEncerrarLeiloesQueComecaramAUmaSemana()
        {
            DateTime diaDaSemanaPassada = new DateTime(1999, 5, 5);

            Leilao leilao1 = new Leilao("Tv de plasma.");
            leilao1.naData(diaDaSemanaPassada);

            Leilao leilao2 = new Leilao("Playstation");
            leilao2.naData(diaDaSemanaPassada);

            List<Leilao> ListaDeLeiloes = new List<Leilao>();

            ListaDeLeiloes.Add(leilao1);
            ListaDeLeiloes.Add(leilao2);

            var dao = new Mock<RepositorioDeLeiloes>();

            dao.Setup(d => d.correntes()).Returns(ListaDeLeiloes);

            EncerradorDeLeilao encerrador = new EncerradorDeLeilao(dao.Object);

            encerrador.encerra();

            Assert.AreEqual(2, encerrador.total);
            Assert.IsTrue(leilao1.encerrado);
            Assert.IsTrue(leilao2.encerrado);
        }

        [Test]
        public void NaoDeveEncerrarLeiloesQueComecaramHoje()
        {
            DateTime Hoje = DateTime.Today;

            Leilao leilao1 = new Leilao("Tv de plasma.");
            leilao1.naData(Hoje);

            Leilao leilao2 = new Leilao("Playstation");
            leilao2.naData(Hoje);

            List<Leilao> ListaDeLeiloes = new List<Leilao>();

            ListaDeLeiloes.Add(leilao1);
            ListaDeLeiloes.Add(leilao2);

            var dao = new Mock<LeilaoDaoFalso>();

            dao.Setup(d => d.correntes()).Returns(ListaDeLeiloes);

            EncerradorDeLeilao encerrador = new EncerradorDeLeilao(dao.Object);

            encerrador.encerra();

            Assert.AreEqual(0, encerrador.total);
            Assert.IsFalse(leilao1.encerrado);
            Assert.IsFalse(leilao2.encerrado);

        }

        [Test]
        public void NaoFazNadaSeNaoTiverLeilao()
        {

            List<Leilao> ListaDeLeiloes = new List<Leilao>();

            var dao = new Mock<LeilaoDaoFalso>();

            dao.Setup(d => d.correntes()).Returns(ListaDeLeiloes);

            EncerradorDeLeilao encerrador = new EncerradorDeLeilao(dao.Object);

            encerrador.encerra();

            Assert.AreEqual(0, encerrador.total);


        }
    }
}
