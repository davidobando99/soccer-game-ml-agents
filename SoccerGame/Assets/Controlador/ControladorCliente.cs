using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Controlador
{
    class ControladorCliente
    {
        public  ControladorCliente()
        {

        }

        public string CoordenadasGameObject(float x, float y)
        {
            string info = "Coordenada x " +x+ "Coordenada y: "+y;
            return info;

        }
    }
}
