using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArrayCalculationTool
{
    public partial class Form1 : Form
    {
        //****************************************************************************************************************************************************
        //Form Constructor
        public Form1()
        {
            InitializeComponent();

            //Initial state for the comboBoxes
            numberOfHubsComboBox.SelectedIndex = 0;
            hub1SizeComboBox.SelectedIndex = 0;
            hub2SizeComboBox.SelectedIndex = 0;
            hub3SizeComboBox.SelectedIndex = 0;
            hub4SizeComboBox.SelectedIndex = 0;
            CalculateButton.Enabled = false;
            clearButton.Enabled = false;

        }
        //****************************************************************************************************************************************************


        //****************************************************************************************************************************************************
        //Graphics Components
        Graphics g;
        Rectangle r;
        Rectangle rVert1;
        Rectangle rVert2;
        Pen p;
        //****************************************************************************************************************************************************


        //****************************************************************************************************************************************************
        //Graphics Methods
        private void drawHubWay(int startX, int width)
        {
            g = CreateGraphics();
            r = new Rectangle(startX, 250, width, width);
            p = new Pen(Brushes.Black);
            p.Width = 5;

            //Draw
            g.DrawRectangle(p, r);
        }

        private void drawHub(int numberOfWays, int startX, int width)
        {

            for (int i = 0; i < numberOfWays; i++)
            {
                drawHubWay(startX, width);

                startX += width;
            }

        }

        private void drawHubs(int[] hubWays)
        {
            int start = 50;
            int width = 50;

            for (int i = 0; i < hubWays.Length; i++)
            {
                drawHub(hubWays[i], start, width);

                start += ((50 * hubWays[i]) + 50);

            }
        }

        private void drawHubs(int[] hubWays, int[] hubStarts, int[] hubEnds)
        {
            int start = 50;
            int width = 50;
            int hubSpacing = 100;
            int midWayofHubWays = width / 2;
            int[] hubS = new int[hubWays.Length];
            int[] hubF = new int[hubWays.Length];

            for (int i = 0; i < hubWays.Length; i++)
            {
                hubStarts[i] = (start + midWayofHubWays);

                drawHub(hubWays[i], start, width);

                start += ((50 * hubWays[i]) + hubSpacing);

                hubEnds[i] = (start - hubSpacing - midWayofHubWays);
            }
        }

        private void drawIntraarrayCable(int startX, int end)
        {
            int length = end - startX;

            g = CreateGraphics();
            r = new Rectangle(startX, 200, length, 5);
            rVert1 = new Rectangle(startX, 200, 5, 45);
            rVert2 = new Rectangle(end, 200, 5, 45);
            p = new Pen(Brushes.Red);
            p.Width = 5;

            g.DrawRectangle(p, r);
            g.DrawRectangle(p, rVert1);
            g.DrawRectangle(p, rVert2);
        }

        private void drawBerthCable(int startX)
        {
            g = CreateGraphics();
            r = new Rectangle((startX - 5), 93, 10, 150);
            p = new Pen(Brushes.Red);
            p.Width = 10;

            g.DrawRectangle(p, r);
        }

        private void drawPlatformCable(int startX)
        {
            g = CreateGraphics();
            r = new Rectangle(startX, 305, 5, 150);
            p = new Pen(Brushes.ForestGreen);
            p.Width = 5;

            g.DrawRectangle(p, r);
        }

        private void drawAllPlatformCables(int[] hubSizes, int[] hubStarts, int[] hubEnds, int wayWidth, int hubSpacing)
        {
            int movingStart = 0;


            for (int j = 0; j < hubSizes.Length; j++)
            {
                if (hubSizes[j] == 2)
                {
                    drawPlatformCable((hubStarts[j]));

                }

                for (int i = 0; i < (hubSizes[j] - 2); i++)
                {


                    if (j == 0)
                    {
                        drawPlatformCable((hubStarts[j] + movingStart));
                    }


                    movingStart += wayWidth;
                    drawPlatformCable((hubStarts[j] + movingStart));

                }

                movingStart = 0;
            }



        }

        private void drawSubseaTransformer(int startX)
        {
            g = CreateGraphics();
            r = new Rectangle((startX - 13), 263, 25, 25);
            p = new Pen(Brushes.Violet);
            p.Width = 5;

            g.DrawRectangle(p, r);
            g.FillRectangle(Brushes.Violet, r);
        }

        private void drawAllSubseaTransformers(int[] hubEnds)
        {
            for (int i = 0; i < hubEnds.Length; i++)
            {
                drawSubseaTransformer(hubEnds[i]);
            }
        }

        private void clearWindow()
        {
            g = CreateGraphics();
            g.Clear(SystemColors.Control);
        }

        private void drawArray(int[] hubSizes, bool subSeaTransformers)
        {
            int[] hubStartPoints = new int[hubSizes.Length];
            int[] hubEndPoints = new int[hubSizes.Length];

            clearWindow();

            //Draw Hubs
            drawHubs(hubSizes, hubStartPoints, hubEndPoints);

            //Draw all Intra-array Cables
            for (int i = 0; i < hubSizes.Length - 1; i++)
            {
                drawIntraarrayCable(hubEndPoints[i], hubStartPoints[i + 1]);
            }

            //Draw Berth Cable
            drawBerthCable(hubEndPoints[(hubSizes.Length - 1)]);

            //Draw all Platform Cables
            drawAllPlatformCables(hubSizes, hubStartPoints, hubEndPoints, 50, 100);

            if (subSeaTransformers)
            {
                //Draw Subsea Transformers in hubs
                drawAllSubseaTransformers(hubEndPoints);
            }

        }
        //****************************************************************************************************************************************************


        //****************************************************************************************************************************************************
        //Data
        TextBox[] platformCableTextBoxes = new TextBox[21];

        TextBox[] intraarrayCableTextBoxes = new TextBox[3];

        private int[] getArrayDataFromTextBox(TextBox textBox)
        {
            string[] arrayLayout = textBox.Text.Split(',');
            int[] hubSizes = new int[arrayLayout.Length];
            int arrayLayoutCount = 0;

            for (int i = 0; i < arrayLayout.Length; i++)
            {

                if (int.TryParse(arrayLayout[i], out hubSizes[i]))
                {
                    //int.TryParse(arrayLayout[i], out hubSizes[i]);
                    arrayLayoutCount++;
                }

                else
                {
                    MessageBox.Show("Invalid data format entered. Please ensure all hub values are numeric and entered in the format (Va1,Val2,Val3,...)",
                        "Invalid Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return hubSizes;

            //if (arrayLayoutCount == arrayLayout.Length)
            //{
            //    output = hubSizes;
            //}

            //else
            //{
            //    MessageBox.Show("Invalid data format entered. Please ensure all hub values are numeric and entered in the format (Va1,Val2,Val3,...)",
            //            "Invalid Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private int[] getDataComboBoxes()
        {
            int hubCount = 0;

            if (hub1SizeComboBox.Enabled) hubCount++;
            if (hub2SizeComboBox.Enabled) hubCount++;
            if (hub3SizeComboBox.Enabled) hubCount++;
            if (hub4SizeComboBox.Enabled) hubCount++;

            int[] arrayLayout = new int[hubCount];
            int[] hubValues = new int[hubCount];

            if (hub1SizeComboBox.Enabled) hubValues[0] = int.Parse(hub1SizeComboBox.Text);
            if (hub2SizeComboBox.Enabled) hubValues[1] = int.Parse(hub2SizeComboBox.Text);
            if (hub3SizeComboBox.Enabled) hubValues[2] = int.Parse(hub3SizeComboBox.Text);
            if (hub4SizeComboBox.Enabled) hubValues[3] = int.Parse(hub4SizeComboBox.Text);

            return hubValues;
        }

        private int[] getDataTextBoxes()
        {
            int count = 0;

            //TODO: Count how many boxes are enabled

            string[] textBoxValuesAsStr = new string[count];
            int[] textBoxValuesAsInt = new int[count];

            //TODO: Work out how to collect this data

            return textBoxValuesAsInt;


        }

        private void groupPlatformCableLengthsTextBoxes(TextBox[] output)
        {
            platformCableTextBoxes[0] = textBox1;
            platformCableTextBoxes[1] = textBox2;
            platformCableTextBoxes[2] = textBox3;
            platformCableTextBoxes[3] = textBox4;
            platformCableTextBoxes[4] = textBox5;
            platformCableTextBoxes[5] = textBox6;
            platformCableTextBoxes[6] = textBox7;
            platformCableTextBoxes[7] = textBox8;
            platformCableTextBoxes[8] = textBox9;
            platformCableTextBoxes[9] = textBox10;
            platformCableTextBoxes[10] = textBox11;
            platformCableTextBoxes[11] = textBox12;
            platformCableTextBoxes[12] = textBox13;
            platformCableTextBoxes[13] = textBox14;
            platformCableTextBoxes[14] = textBox15;
            platformCableTextBoxes[15] = textBox16;
            platformCableTextBoxes[16] = textBox17;
            platformCableTextBoxes[17] = textBox18;
            platformCableTextBoxes[18] = textBox19;
            platformCableTextBoxes[19] = textBox20;
            platformCableTextBoxes[20] = textBox30;

        }

        private void groupIntraarrayCableLengthTextBoxes(TextBox[] output)
        {
            output[0] = textBox21;
            output[1] = textBox22;
            output[2] = textBox23;
        }

        private void hideAllPlatformCableBoxes(TextBox[] platformCableBoxes)
        {
            for (int i = 0; i < platformCableBoxes.Length; i++)
            {
                platformCableBoxes[i].Visible = false;
            }
        }

        private void setPlatformCableTextBoxesVisablility(TextBox[] textBoxes, int[] hubWays)
        {
            int platformCables = 0;

            if (hubWays.Length == 1)
            {
                platformCables = hubWays.Sum() - 1;
            }

            else
            {
                platformCables = hubWays.Sum() - (2 * hubWays.Length) + 1;
            }

            hideAllPlatformCableBoxes(textBoxes);

            for (int i = 0; i < (platformCables); i++)
            {
                textBoxes[i].Visible = true;
            }
        }

        private void setIntraArrayTextBoxesVisability(TextBox[] textBoxes, int[] hubways)
        {
            hideAllPlatformCableBoxes(textBoxes);

            if (hubways.Length > 1)
            {
                for (int i = 0; i < (hubways.Length - 1); i++)
                {
                    textBoxes[i].Visible = true;
                }
            }

        }

        private int[] getPlatformCableLengths()
        {
            int platformCablesCount = 0;

            //Textbox array of platform cables (all of them)
            groupPlatformCableLengthsTextBoxes(platformCableTextBoxes);

            //Set count to the number of cables in specific array
            foreach (TextBox textbox in platformCableTextBoxes)
            {
                if (textbox.Visible) platformCablesCount++;
            }

            int[] platformCableLengths = new int[platformCablesCount];

            for (int i = 0; i < platformCablesCount; i++)
            {
                platformCableLengths[i] = int.Parse(platformCableTextBoxes[i].Text);
            }

            return platformCableLengths;
        }

        private int[] getIntraarrayCableLengths()
        {
            int intraArrayCableCount = 0;

            foreach (TextBox textbox in intraarrayCableTextBoxes)
            {
                if (textbox.Visible) intraArrayCableCount++;
            }

            int[] intraArrCblLengths = new int[intraArrayCableCount];

            for (int i = 0; i < intraArrayCableCount; i++)
            {
                intraArrCblLengths[i] = int.Parse(intraarrayCableTextBoxes[i].Text);
            }

            return intraArrCblLengths;
        }

        private void displayPlatformCblRes(Label label, double[] platformCableRes)
        {
            label.Text = "Resistance:\n";

            for (int i = 0; i < platformCableRes.Length; i++)
            {
                label.Text = String.Format(label.Text + "[{0}] {1:0.00000} Ω {2}", i + 1, platformCableRes[i], Environment.NewLine);
            }
        }

        private void displayPlatformRatedPwr(Label label, int platformRatedPower)
        {
            label.Text = "Platform Rated Power:\n";

            label.Text = String.Format(label.Text + "{0} kW", platformRatedPower);

        }

        private void displayTransformerEff(Label label, double onboardTransformerEff)
        {
            label.Text = "Transformer Efficiency:\n" +
                         String.Format("{0:0.00}%", onboardTransformerEff * 100);
        }

        private void displayPlatformExportedPwr(Label label, double exportedPower)
        {
            label.Text = "Exported Power:\n" +
                          String.Format("{0:0.00}kW", exportedPower);
        }

        private void displayPlatformCableCurrent(Label label, double current)
        {
            label.Text = "Exported Power:\n" +
                          String.Format("{0:0.00}A", current);
        }

        private void displayPlatformCblLosses(Label label, double[] platformCableLosses)
        {
            label.Text = "Losses:\n";

            for (int i = 0; i < platformCableLosses.Length; i++)
            {
                label.ForeColor = Color.Red;
                label.Text = label.Text + String.Format("[{0}] {1:0.00} kW {2}", i + 1, platformCableLosses[i], Environment.NewLine);
            }

        }

        private void displayHubDeliveredPwr(Label label, double[] powerDelHubs)
        {
            label.Text = "Delivered Per:\n";

            for (int i = 0; i < powerDelHubs.Length; i++)
            {
                label.Text = label.Text + String.Format("[{0}] {1:0.00} kW {2}", i + 1, powerDelHubs[i], Environment.NewLine);
            }
        }

        private void displaySubseaTransLosses(Label label, double[] subseaTransLosses)
        {
            label.Text = "Losses:\n";

            for (int i = 0; i < subseaTransLosses.Length; i++)
            {
                label.ForeColor = Color.Red;
                label.Text = label.Text + String.Format("[{0}] {1:0.00} kW {2}", i + 1, subseaTransLosses[i], Environment.NewLine);
            }

        }

        private void displayHubPwrAfterTrans(Label label, double[] powerDelHubs)
        {
            label.Text = "Pwr After Trans:\n";

            for (int i = 0; i < powerDelHubs.Length; i++)
            {
                label.Text = label.Text + String.Format("[{0}] {1:0.00} kW {2}", i + 1, powerDelHubs[i], Environment.NewLine);
            }
        }

        private void displayIntArrCblRes(Label label, double[] intArrCableRes)
        {
            label.Text = "Resistance:\n";

            for (int i = 0; i < intArrCableRes.Length; i++)
            {
                label.Text = String.Format(label.Text + "[{0}] {1:0.00000} Ω {2}", i + 1, intArrCableRes[i], Environment.NewLine);
            }
        }

        private void displayIntArrCblInfo(Label lossesLabel, Label currentLabel, double[] losses, double[] current)
        {
            lossesLabel.Text = "Losses:\n";

            for (int i = 0; i < losses.Length; i++)
            {
                lossesLabel.ForeColor = Color.Red;
                lossesLabel.Text = lossesLabel.Text + String.Format("[{0}] {1:0.00} kW {2}", i + 1, losses[i], Environment.NewLine);
            }

            currentLabel.Text = "Current:\n";

            for (int i = 0; i < current.Length; i++)
            {
                currentLabel.Text = currentLabel.Text + String.Format("[{0}] {1:0.00} A {2}", i + 1, current[i], Environment.NewLine);
            }

        }

        private void displayBerthCableRes(Label resistanceLabel, double resistance)
        {
            resistanceLabel.Text = "Resistance:\n";

            resistanceLabel.Text = resistanceLabel.Text + String.Format("[{0}] {1:0.00} Ω {2}", 1, resistance, Environment.NewLine);

        }

        private void displayBerthCableCurent(Label currentLabel, double current)
        {
            currentLabel.Text = "Current:\n";

            currentLabel.Text = currentLabel.Text + String.Format("[{0}] {1:0.00} A {2}", 1, current, Environment.NewLine);

        }

        private void displayBerthCableLosses(Label lossesLabel, double losses)
        {
            lossesLabel.Text = "Losses:\n";

            lossesLabel.ForeColor = Color.Red;
            lossesLabel.Text = lossesLabel.Text + String.Format("[{0}] {1:0.00} kW {2}", 1, losses, Environment.NewLine);

        }

        private void displayArraySummary(Label labelPower, Label labelLosses, double arrayRatingTotal, double totalDelPwr, double totalSystemLoss, double totalSysLossPercent, double systemEffPercent)
        {
            labelPower.Text = "Total Array Rating:\n";
            labelPower.Text = labelPower.Text + String.Format("{0:0.00} MW {1}", (arrayRatingTotal / 1000), Environment.NewLine);
            labelPower.Text += "Total Del Pwr:\n";
            labelPower.Text = labelPower.Text + String.Format("{0:0.00} MW {1}", (totalDelPwr / 1000), Environment.NewLine);
            labelLosses.Text = "Total Sys Losses:\n";
            labelLosses.Text = labelLosses.Text + String.Format("{0:0.00} kW {1}", totalSystemLoss, Environment.NewLine);
            labelPower.Text += "System Efficiency:\n";
            labelPower.Text = labelPower.Text + String.Format("{0:0.00}% {1}", totalSysLossPercent, Environment.NewLine);
            labelLosses.Text = labelLosses.Text + String.Format("{0:0.00}% {1}", systemEffPercent, Environment.NewLine);
        }

        private void clearAllResults()
        {
            //Clear all Labels
            platformCableResistancesLabel.Text = "";
            platformCableLossesLabel.Text = "";
            platformPwrRatingLabel.Text = "";
            onboardTransEffLabel.Text = "";
            platformExportPwrLabel.Text = "";
            platformExportedCurrentLabel.Text = "";
            hubsDeliveredPwrLabel.Text = "";
            hubsLossesLabel.Text = "";
            hubsPwrAfterTransLossLabel.Text = "";
            intArrCableResLabel.Text = "";
            intArrLossesLabel.Text = "";
            intArrCurrentLabel.Text = "";
            BerthCableResLabel.Text = "";
            berthCableLossesLabel.Text = "";
            berthCableCurrentLabel.Text = "";
            systemSummaryLabel.Text = "";
            systemSummaryLossesLabel.Text = "";

        }

        private void toggleInputs(bool toggle)
        {
            //Input TextBoxes
            groupBox13.Enabled = toggle;
            groupBox16.Enabled = toggle;
            groupBox1.Enabled = toggle;
            groupBox2.Enabled = toggle;
            groupBox3.Enabled = toggle;
            groupBox5.Enabled = toggle;
            groupBox7.Enabled = toggle;
            groupBox8.Enabled = toggle;

            //Input ComboBoxes
            if (toggle == false)
            {
                numberOfHubsComboBox.Enabled = false;
                hub1SizeComboBox.Enabled = false;
                hub2SizeComboBox.Enabled = false;
                hub3SizeComboBox.Enabled = false;
                hub4SizeComboBox.Enabled = false;
            }
            else if (toggle)
            {
                numberOfHubsComboBox.Enabled = true;

                switch (numberOfHubsComboBox.Text)
                {
                    case "1":
                        hub1SizeComboBox.Enabled = true;
                        hub2SizeComboBox.Enabled = false;
                        hub3SizeComboBox.Enabled = false;
                        hub4SizeComboBox.Enabled = false;
                        break;

                    case "2":
                        hub1SizeComboBox.Enabled = true;
                        hub2SizeComboBox.Enabled = true;
                        hub3SizeComboBox.Enabled = false;
                        hub4SizeComboBox.Enabled = false;
                        break;

                    case "3":
                        hub1SizeComboBox.Enabled = true;
                        hub2SizeComboBox.Enabled = true;
                        hub3SizeComboBox.Enabled = true;
                        hub4SizeComboBox.Enabled = false;
                        break;

                    case "4":
                        hub1SizeComboBox.Enabled = true;
                        hub2SizeComboBox.Enabled = true;
                        hub3SizeComboBox.Enabled = true;
                        hub4SizeComboBox.Enabled = true;
                        break;

                    default:
                        break;
                }
            }

            //Subsea Transformer RadioButtons
            subseaTransformerYesRadioButton.Enabled = toggle;
            subseaTransformerNoRadioButton.Enabled = toggle;
        }
        //****************************************************************************************************************************************************


        //****************************************************************************************************************************************************
        //Array Components

        //TODO: Implement a way to store cables, transformers, and all array components data.
        //Example: Cable with length, CSA, resisitity etc.... Maybe lists...?

        //Cables
        Cable platformCable = new Cable();
        Cable intraarrayCable = new Cable();
        Cable berthCable = new Cable();

        //Transformers
        Transformer onBoardTransformer = new Transformer();
        Transformer subseaTransformer = new Transformer();

        //****************************************************************************************************************************************************


        //****************************************************************************************************************************************************
        //Calculations

        //TODO: Implement array calculations for current, losses and delivered power.
        private void setCableData(Cable cable, TextBox resistivityTextBox, TextBox csaTextBox)
        {
            cable.Resistivity = double.Parse(resistivityTextBox.Text);
            cable.CSA = int.Parse(csaTextBox.Text);
        }

        private void setAllCables()
        {
            setCableData(platformCable, pltCblResTextBox, pltCblCsaTextBox);
            setCableData(intraarrayCable, intArrCblResTextBox, intArrCblCsaTextBox);
            setCableData(berthCable, berthCableResTextBox, berthCableCsaTextBox);
        }

        private double[] calculatePlatformCableResistances()
        {
            setAllCables();

            int[] platformCableLengths = getPlatformCableLengths();
            double[] platformCableResistances = new double[platformCableLengths.Length];

            for (int i = 0; i < platformCableLengths.Length; i++)
            {
                platformCableResistances[i] = platformCableLengths[i] * platformCable.Resistivity;
            }

            return platformCableResistances;
        }

        private double[] calculateIntraarrayCableResistances()
        {
            setAllCables();

            int[] cableLengths = getIntraarrayCableLengths();
            double[] cableResistances = new double[cableLengths.Length];

            for (int i = 0; i < cableLengths.Length; i++)
            {
                cableResistances[i] = cableLengths[i] * intraarrayCable.Resistivity;
            }

            return cableResistances;
        }

        private int calculatePlatformPwrRating(TextBox sitPwrRatingTextBox, TextBox numberOfSitTextBox)
        {
            int platformPwrRating = 0;

            platformPwrRating = int.Parse(sitPwrRatingTextBox.Text) * int.Parse(numberOfSitsTextBox.Text);

            return platformPwrRating;
        }

        private void setAllTransformers()
        {
            //onboard
            onBoardTransformer.ActPwrRating = int.Parse(onboardTransActPwrTextBox.Text);
            onBoardTransformer.MaxLosses = int.Parse(onboardTransMaxLossTextBox.Text);
            onBoardTransformer.LV_Voltage = double.Parse(onboardTransLvTextBox.Text);
            onBoardTransformer.HV_Voltage = double.Parse(onboardTransHvTextBox.Text);

            //Subsea
            subseaTransformer.ActPwrRating = int.Parse(subseaActPwrTextBox.Text);
            subseaTransformer.MaxLosses = int.Parse(subseaTransMaxLossTextBox.Text);
            subseaTransformer.LV_Voltage = double.Parse(subseaTransLvTextBox.Text);
            subseaTransformer.HV_Voltage = double.Parse(subseaTransHvTextBox.Text);
            subseaTransformer.Efficiency = 1 - ((float)subseaTransformer.MaxLosses / (float)subseaTransformer.ActPwrRating);
        }

        private double calculateOnboardTransEff()
        {
            double transformerEff = 0;

            setAllTransformers();

            transformerEff = 1 - ((double)onBoardTransformer.MaxLosses / (double)onBoardTransformer.ActPwrRating);

            return transformerEff;
        }

        private double calculatePlatformExportedPower(int ratedPwr, Transformer onboardTrans)
        {
            double exportedValue = 0;

            setAllTransformers();

            exportedValue = (double)ratedPwr * calculateOnboardTransEff();

            return exportedValue;
        }

        private double calculateCurrent(double power, double voltage)
        {
            double current = 0;

            current = (power * 1000) / (0.95 * voltage * Math.Sqrt(3));

            return current;
        }

        private double[] calculateCableLosses(double[] cableResistances, double cableCurrent)
        {
            double[] losses = new double[cableResistances.Length];

            for (int i = 0; i < cableResistances.Length; i++)
            {
                //Possible Error
                //losses[i] = ((Math.Sqrt(3) * (cableCurrent * cableCurrent) * cableResistances[i]) / 1000);

                losses[i] = ((3 * (cableCurrent * cableCurrent) * cableResistances[i]) / 1000);
            }

            return losses;
        }

        private int[] calculatePlatformsPerHub(int[] hubWays)
        {
            int[] platformCablesPerHub = new int[hubWays.Length];

            for (int i = 0; i < hubWays.Length; i++)
            {
                if (i == 0)
                {
                    platformCablesPerHub[i] = hubWays[i] - 1;
                }

                else
                {
                    platformCablesPerHub[i] = hubWays[i] - 2;
                }
            }


            return platformCablesPerHub;
        }

        private double[] calculatePwrAfTPltCbl(double platformExportedPwr, double[] platformCableLosses)
        {
            double[] pwrAfterLosses = new double[platformCableLosses.Length];

            for (int i = 0; i < platformCableLosses.Length; i++)
            {
                pwrAfterLosses[i] = platformExportedPwr - platformCableLosses[i];
            }

            return pwrAfterLosses;
        }

        private double[] calculateHubsDeliveredPwr(int[] platformsPerHub, double[] powerAfterPlatformCable)
        {
            double[] powerDel = new double[platformsPerHub.Length];

            for (int i = 0; i < powerDel.Length; i++)
            {
                powerDel[i] = (double)platformsPerHub[i] * powerAfterPlatformCable[i];
            }

            return powerDel;
        }

        private double[] calculateSubseaTransformerLosses(double[] powerDelHubs)
        {
            double[] losses = new double[powerDelHubs.Length];

            for (int i = 0; i < losses.Length; i++)
            {
                if (subseaTransformerYesRadioButton.Checked) losses[i] = powerDelHubs[i] * (1 - subseaTransformer.Efficiency);

                else losses[i] = 0;

            }

            return losses;
        }

        private double[] calculateSubseaPwrAfterTrans(double[] hubsDelPwr, double[] transfomerLosses)
        {
            double[] pwrAfterTrans = new double[transfomerLosses.Length];

            for (int i = 0; i < pwrAfterTrans.Length; i++)
            {
                pwrAfterTrans[i] = hubsDelPwr[i] - transfomerLosses[i];
            }

            return pwrAfterTrans;
        }

        private void calculateIntraArray(double[] hubPwrAftTras, double[] intraArrRes, Transformer transformer, out double[] intArrCurrent, out double[] intArrLosses, out double[] hubTotalPwrs)
        {
            double[] hubPwrs = hubPwrAftTras;
            double[] intArrayCurrents = new double[intraArrRes.Length];
            double[] intArrayLosses = new double[intraArrRes.Length];

            for (int i = 0; i < intraArrRes.Length; i++)
            {
                //Int Current
                intArrayCurrents[i] = calculateCurrent(hubPwrs[i], subseaTransformer.HV_Voltage);

                //Int losses
                //Possible Error
                // intArrayLosses[i] = (Math.Sqrt(3) * ((intArrayCurrents[i] * intArrayCurrents[i]) * intraArrRes[i])) / 1000;
                intArrayLosses[i] = (3 * ((intArrayCurrents[i] * intArrayCurrents[i]) * intraArrRes[i])) / 1000;

                //Power leaving hub
                hubPwrs[i] -= (intArrayLosses[i]);

                //next hub on pwr sum
                hubPwrs[i + 1] += hubPwrs[i];
            }

            hubTotalPwrs = hubPwrs;
            intArrCurrent = intArrayCurrents;
            intArrLosses = intArrayLosses;
        }

        private double calculateBerthCableRes()
        {
            double res = 0;

            setAllCables();

            res = berthCable.Resistivity * double.Parse(berthCableLengthTextBox.Text);

            return res;
        }

        private double calculateBerthCableLosses(double current, double resistance)
        {
            double losses = 0;
            //Possible Error
            //losses = Math.Sqrt(3) * ((current * current) * resistance);

            losses = 3 * ((current * current) * resistance);

            return losses / 1000;
        }
        //****************************************************************************************************************************************************


        //****************************************************************************************************************************************************
        //Event Handles
        private void DrawButton_Click(object sender, EventArgs e)
        {
            //Read User input Data
            int[] hubWays = getDataComboBoxes();
            bool toggleSubseaTransfomers = subseaTransformerYesRadioButton.Checked;

            //Draw Array Layout
            drawArray(hubWays, toggleSubseaTransfomers);

            //Set Textboxes
            groupPlatformCableLengthsTextBoxes(platformCableTextBoxes);
            setPlatformCableTextBoxesVisablility(platformCableTextBoxes, hubWays);

            groupIntraarrayCableLengthTextBoxes(intraarrayCableTextBoxes);
            setIntraArrayTextBoxesVisability(intraarrayCableTextBoxes, hubWays);

            //Enable buttons
            CalculateButton.Enabled = true;
            clearButton.Enabled = true;

            //Eneable Inputs
            toggleInputs(true);

            //Disable button
            DrawButton.Enabled = false;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            //TODO: Fix the fact textBox[] is not defined, if this is clicked before drawing etc. it will throw an exception

            g = CreateGraphics();
            g.Clear(SystemColors.Control);

            //Hide all platform cable input textboxes
            hideAllPlatformCableBoxes(platformCableTextBoxes);
            hideAllPlatformCableBoxes(intraarrayCableTextBoxes);

            //Disable calculate button
            CalculateButton.Enabled = false;

            //Clear results
            clearAllResults();

            //Toggle buttons
            DrawButton.Enabled = true;
            clearButton.Enabled = false;

            //Toggel inputs
            toggleInputs(true);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (numberOfHubsComboBox.Text)
            {
                case "1":
                    hub1SizeComboBox.Enabled = true;
                    hub2SizeComboBox.Enabled = false;
                    hub3SizeComboBox.Enabled = false;
                    hub4SizeComboBox.Enabled = false;
                    break;

                case "2":
                    hub1SizeComboBox.Enabled = true;
                    hub2SizeComboBox.Enabled = true;
                    hub3SizeComboBox.Enabled = false;
                    hub4SizeComboBox.Enabled = false;
                    break;

                case "3":
                    hub1SizeComboBox.Enabled = true;
                    hub2SizeComboBox.Enabled = true;
                    hub3SizeComboBox.Enabled = true;
                    hub4SizeComboBox.Enabled = false;
                    break;

                case "4":
                    hub1SizeComboBox.Enabled = true;
                    hub2SizeComboBox.Enabled = true;
                    hub3SizeComboBox.Enabled = true;
                    hub4SizeComboBox.Enabled = true;
                    break;

                default:
                    break;
            }
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            //Disable button
            CalculateButton.Enabled = false;

            #region Platform Cable Calcs
            //Calculate onboard transformer efficiency
            double onboardTransformerEfficiency = calculateOnboardTransEff();
            displayTransformerEff(onboardTransEffLabel, onboardTransformerEfficiency);

            //Calculate each platform's Rated power
            int platformPowerRating = calculatePlatformPwrRating(sitRatingTextBox, numberOfSitsTextBox);
            displayPlatformRatedPwr(platformPwrRatingLabel, platformPowerRating);

            //Calculate exported power after platform trans
            double platformExportedPower = calculatePlatformExportedPower(platformPowerRating, onBoardTransformer);
            displayPlatformExportedPwr(platformExportPwrLabel, platformExportedPower);

            //Calculate Platform Cable Resistances
            double[] platformCablesRes = calculatePlatformCableResistances();
            displayPlatformCblRes(platformCableResistancesLabel, platformCablesRes);

            //Calculate platform cable current
            double platformCableCurrent = calculateCurrent(platformExportedPower, onBoardTransformer.HV_Voltage);
            displayPlatformCableCurrent(platformExportedCurrentLabel, platformCableCurrent);

            //Calculate platform cable losses
            double[] platformCableLosses = calculateCableLosses(platformCablesRes, platformCableCurrent);
            displayPlatformCblLosses(platformCableLossesLabel, platformCableLosses);

            //Power After Platform Cable Losses
            double[] powerAfterPlatformCableLosses = calculatePwrAfTPltCbl(platformExportedPower, platformCableLosses);
            #endregion

            #region Hub Calculations
            //Calculate platforms per hub
            int[] platformsPerHub = calculatePlatformsPerHub(getDataComboBoxes());

            //Calculate Hubs Totals Delivered Power
            double[] powerDeliveredHubs = calculateHubsDeliveredPwr(platformsPerHub, powerAfterPlatformCableLosses);
            displayHubDeliveredPwr(hubsDeliveredPwrLabel, powerDeliveredHubs);

            //Calculate hub losses
            double[] subseaTransLosses = calculateSubseaTransformerLosses(powerDeliveredHubs);
            displaySubseaTransLosses(hubsLossesLabel, subseaTransLosses);

            //Display total exported hub pwr
            double[] HubPowerAfterTrans = calculateSubseaPwrAfterTrans(powerDeliveredHubs, subseaTransLosses);
            displayHubPwrAfterTrans(hubsPwrAfterTransLossLabel, HubPowerAfterTrans);
            #endregion

            #region Intra-array Cables
            //Calculate intra-array resistances
            double[] intraArrayCableRes = calculateIntraarrayCableResistances();
            displayIntArrCblRes(intArrCableResLabel, intraArrayCableRes);

            //Intra Array Cables & Losses
            double[] intraArrCurrents;
            double[] intraArrLosses;
            double[] hubTotalPwrsHV;
            calculateIntraArray(HubPowerAfterTrans, intraArrayCableRes, subseaTransformer, out intraArrCurrents, out intraArrLosses, out hubTotalPwrsHV);
            displayIntArrCblInfo(intArrLossesLabel, intArrCurrentLabel, intraArrLosses, intraArrCurrents);
            #endregion

            #region Berth Cable
            //Calculate Berth Cable Resistance
            double berthCableResistance = calculateBerthCableRes();
            displayBerthCableRes(BerthCableResLabel, berthCableResistance);

            //Calculate Berth Cable Current
            double berthCableCurrent = calculateCurrent(hubTotalPwrsHV[hubTotalPwrsHV.Length - 1], subseaTransformer.HV_Voltage);
            displayBerthCableCurent(berthCableCurrentLabel, berthCableCurrent);

            //Calculate Berth Cable Losses
            double berthCableLosses = calculateBerthCableLosses(berthCableCurrent, berthCableResistance);
            displayBerthCableLosses(berthCableLossesLabel, berthCableLosses);

            //Calculate Final Delivered Power
            double finalDelPwr = hubTotalPwrsHV[hubTotalPwrsHV.Length - 1] - berthCableLosses;

            //Display System Summary
            double arrayTotalRating = platformPowerRating * platformsPerHub.Sum();
            double totalArrayLosses = arrayTotalRating - finalDelPwr;
            double arrayLossPercent = (finalDelPwr / arrayTotalRating) * 100;
            double arrayEffPercent = (1 - (finalDelPwr / arrayTotalRating)) * 100;
            displayArraySummary(systemSummaryLabel, systemSummaryLossesLabel, arrayTotalRating, finalDelPwr, totalArrayLosses, arrayLossPercent, arrayEffPercent);
            #endregion        }

            //Disable all input
            toggleInputs(false);            
        }
        //****************************************************************************************************************************************************
    }
}
