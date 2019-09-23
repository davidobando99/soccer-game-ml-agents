using Assets.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Controlador
{
    class ControladorCliente
        
    {
        private Cliente Cliente;
        public  ControladorCliente()
        {
            Cliente = new Cliente();

        }

        public void Send(float x, float y, int unitID)
        {
            String info = "Moving|" + unitID + "|" + x + "|" + y + "|";
            Cliente.Send(info);
        }
        

    }
}
