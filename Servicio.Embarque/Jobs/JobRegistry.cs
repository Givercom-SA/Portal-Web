using FluentScheduler;
using Servicio.Embarque.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Jobs
{
    public class JobRegistry : Registry
    {
        public JobRegistry(ProcesoBusinessLogic procesoBusinessLogic)
        {
            Schedule(new MemoJob(procesoBusinessLogic))
                .ToRunNow()
                .AndEvery(1)//Minutos
                .Minutes(); 

            Schedule(new FacturaJob(procesoBusinessLogic))
               .ToRunNow()
               .AndEvery(2)//Minutos
               .Minutes();

            Schedule(new NotificacionArriboJob(procesoBusinessLogic))
           .ToRunNow()
           .AndEvery(3)//Minutos
           .Minutes();

        }

       


    }
}
