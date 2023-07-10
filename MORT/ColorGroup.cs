namespace MORT
{
    public class ColorGroup
    {
        int valueR = 0, valueG = 0, valueB = 0;
        int valueV1 = 0, valueV2 = 0, valueS1 = 0, valueS2 = 0; //S1~S2, V1~V2

        public ColorGroup()
        {
            valueR = 0;
            valueG = 0;
            valueB = 0;
            valueV1 = 0;
            valueV2 = 0;
            valueS1 = 0;
            valueS2 = 0;
        }

        public void SetHSV(QuickSettingData.HSVData data)
        {
            this.valueS1 = data.startS;
            this.valueS2 = data.endS;

            this.valueV1 = data.startV;
            this.valueV2 = data.endV;
        }


        public void checkHSVRange()
        {
            if (valueS1 > valueS2)
            {
                int temp = valueS1;
                valueS1 = valueS2;
                valueS2 = temp;

            }
            if (valueV1 > valueV2)
            {
                int temp = valueV1;
                valueV1 = valueV2;
                valueV2 = temp;
            }
        }


        public void setRGBValuse(int newR, int newG, int newB)
        {
            valueR = newR;
            valueG = newG;
            valueB = newB;
        }
        public void setHSVValuse(int newS1, int newS2, int newV1, int newV2)
        {
            valueV1 = newV1;
            valueV2 = newV2;
            valueS1 = newS1;
            valueS2 = newS2;
        }
        public int getValueR()
        {
            return valueR;
        }
        public int getValueG()
        {
            return valueG;
        }
        public int getValueB()
        {
            return valueB;
        }


        public int getValueV1()
        {
            return valueV1;
        }


        public int getValueV2()
        {
            return valueV2;
        }


        public int getValueS1()
        {
            return valueS1;
        }


        public int getValueS2()
        {
            return valueS2;
        }

    }
}
