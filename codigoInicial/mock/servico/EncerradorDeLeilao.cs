using mock.dominio;
using mock.infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mock.servico
{
    public class EncerradorDeLeilao
    {
        public int total { get; private set; }

        private RepositorioDeLeiloes dao;

        public EncerradorDeLeilao(RepositorioDeLeiloes dao)
        {
            total = 0;
            this.dao = dao;
        }

        public virtual void encerra()
        {            
            List<Leilao> todosLeiloesCorrentes = dao.correntes();

            foreach (var l in todosLeiloesCorrentes)
            {

                if (comecouSemanaPassada(l))
                {

                    l.encerra();
                    total++;
                    dao.atualiza(l);

                }
            }
        }


        private bool comecouSemanaPassada(Leilao leilao)
        {

            return diasEntre(leilao.data, DateTime.Now) >= 7;

        }

        private int diasEntre(DateTime inicio, DateTime fim)
        {
            int dias = (int)(fim - inicio).TotalDays;

            return dias;
        }

    }
}
