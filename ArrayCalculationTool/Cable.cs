using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayCalculationTool
{
    class Cable
    {
        private int length;
        private double resisitvity;
        private int csa;


        public int Length
        {
            get { return length; }
            set { length = value; }
        }
        public double Resistivity
        {
            get { return resisitvity; }
            set { resisitvity = value / 1000; }
        }
        public int CSA
        {
            get { return csa; }
            set { csa = value; }
        }

        public double calculateResitance()
        {
            double resitance = 0;

            return resitance;
        }

    }
}
