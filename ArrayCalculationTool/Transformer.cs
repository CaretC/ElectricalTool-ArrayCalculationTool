using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayCalculationTool
{
    class Transformer
    {
        private double lv_Voltage;
        private double hv_Voltage;
        private int actPwrRating;
        private int reaPwrRating;
        private float efficiency;
        private int maxLosses;

        public double LV_Voltage
        {
            get { return lv_Voltage; }
            set { lv_Voltage = value * 1000; }
        }
        public double HV_Voltage
        {
            get { return hv_Voltage; }
            set { hv_Voltage = value * 1000; }
        }
        public int ActPwrRating
        {
            get { return actPwrRating; }
            set { actPwrRating = (value * 1000); }
        }
        public int ReaPwrRating
        {
            get { return reaPwrRating; }
            set { reaPwrRating = value; }
        }
        public float Efficiency
        {
            get { return efficiency; }
            set { efficiency = value; }
        }
        public int MaxLosses
        {
            get { return maxLosses; }
            set { maxLosses = value; }
        }

        //TODO: Efficienct and losses.....could you stick the table in...?

    }
}
