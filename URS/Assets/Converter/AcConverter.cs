namespace Converter
{
    using System;
    using System.Text;

    /// <summary>
    /// ��ũ��Ʈ ��ȣȭ�� ���� ���Ǹ� �����ϱ� ���� Interface�Դϴ�.
    /// ���Ŀ� ��ȣ/��ȭȭ�� ���� ������ �� �� �������� �� �ս��� ��ü�� �� �ֵ��� �ϱ����� �������̽��� ����
    /// </summary>
    public interface IStrTransformer
    {
        string Encryption(string str);
        string Decryption(string str);
        char ShiftChar(char ch, int range);
    }

    public class EasyStrConverter : IStrTransformer
    {
        private StringBuilder _strBuilder = new StringBuilder();

        public string Encryption(string str)
        {
            // If word is null, we just return null.
            if (string.IsNullOrEmpty(str))
                return null;

            _strBuilder.Clear();

            var length = str.Length;
            for (var i = 0; i < length; i++)
            {
                var ch = str[i];
                var charCypher = ShiftChar(ch, length);
                _strBuilder.Append(Convert.ToString(charCypher));
            }

            return _strBuilder.ToString();
        }

        public string Decryption(string str)
        {
            // If word is null, we just return null.
            if (string.IsNullOrEmpty(str))
                return null;

            _strBuilder.Clear();

            var length = str.Length;
            for (var i = 0; i < length; i++)
            {
                var ch = str[i];
                var charCypher = ShiftChar(ch, -length);
                _strBuilder.Append(Convert.ToString(charCypher));
            }

            return _strBuilder.ToString();
        }

        public char ShiftChar(char ch, int range)
        {
            if (char.IsUpper(ch))
            {
                return ShiftChar(ch, range, min: 65, max: 90); // min : A, max Z
            }

            if (char.IsLower(ch))
            {
                return ShiftChar(ch, range, min: 97, max: 122); // min : a, max z
            }

            if (char.IsNumber(ch))
            {
                return ShiftChar(ch, range, min: 48, max: 57); // min : 0, max 9
            }

            return ch;
        }

        private char ShiftChar(char ch, int range, int min, int max)
        {
            var minMaxGap = max - min;
            var rangeOverFlow = range % minMaxGap;
            if (rangeOverFlow == 0)
            {
                if (range > 0)
                {
                    rangeOverFlow += 2;
                }

                if (range < 0)
                {
                    rangeOverFlow -= 2;
                }
            }

            var shiftGap = ch + rangeOverFlow;

            if (shiftGap > max)
            {
                var maxOverFlow = shiftGap % max;
                var moveValue = min + maxOverFlow - 1;

                return Convert.ToChar(moveValue);
            }

            if (shiftGap < min)
            {
                var maxOverFlow = min % shiftGap;
                var moveValue = max - maxOverFlow + 1;

                return Convert.ToChar(moveValue);
            }

            return Convert.ToChar(shiftGap);
        }
    }

}
