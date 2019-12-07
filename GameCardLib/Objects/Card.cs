using System;
using System.Collections.Generic;
using System.Text;

namespace GameCardLib
{
    public class Card
    {
        private Suites suite;
        private int _value;
        private string imageName; //location of image
        private bool faceUp;
        private readonly string locationOfImages = ""; //"../cards/";

        public Card(Suites suite, int value)
        {
            this.suite = suite;
            _value = value;
            GetImage();
            Suites a = (Suites)1;
        }

        private void GetImage()
        {
            char suiteMark = suite.ToString()[0];
            string value = _value.ToString();
            if (_value == 1)
                value = "A";
            else if (_value == 11)
                value = "J";
            else if (_value == 12)
                value = "Q";
            else if (_value == 13)
                value = "K";
            imageName = locationOfImages + value + suiteMark + ".jpg";

        }

        #region Get Set
        public Suites Suite
        {
            get
            {
                return suite;
            }
            set
            {
                suite = value;
            }
        }

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value >= 2)
                    _value = value;
            }
        }

        public string SuiteString
        {
            get
            {
                return suite.ToString();
            }
        }

        public string ValueString
        {
            get
            {
                if (_value == 1)
                    return "Ace";
                else if (_value == 11)
                    return "jack";
                else if (_value == 12)
                    return "Queen";
                else if (_value == 13)
                    return "King";
                else
                    return _value.ToString();
            }
        }

        public string Image
        {
            get
            {
                return imageName;
            }
            set
            {
                if (value.Length > 0)
                    imageName = value;
            }
        }

        public bool FaceUp
        {
            get
            {
                return faceUp;
            }
            set
            {
                faceUp = value;
            }
        }

        #endregion
    }
}
