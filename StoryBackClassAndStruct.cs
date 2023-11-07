using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace JSNi.Struct
{
    
    [Serializable]
    struct Img
    {
        [SerializeField, Header("圖片")]
        Sprite _img;
        public Sprite _Img { get { return _img; } set { _img = value; } }
        [SerializeField, Header("坐標")]
        Vector3 _pos;
        public Vector3 Pos { get { return _pos; } set { _pos = value; } }

    }
    [Serializable]
    struct StoryStruct
    {
        [SerializeField, Header("讀取圖片")]
        public Img[] _img;
        [Header("Storys"), Multiline]
        public string[] _storys;
        public int nowImgIndex;

    }
}
namespace JSNi.SystemFunction
{
    static class RegexFunction
    {
        /// 新增新的切割字串時，只要新增 static string以及KnifeStringChange中的條件即可
        #region 切割動作，已完成，幾乎不用修改
        /// <summary>
        /// 輸入主目標以及副要目標，傳回正規表達式
        /// </summary>
        /// <param name="mainKnife">主目標</param>
        /// <param name="viceKnife">副要目標，可以複數</param>
        /// <returns></returns>
        static public Regex ClipStringKnife(string mainKnife, string[] viceKnife)
        {
            return new Regex(FinalKnife(mainKnife, viceKnife));
        }
        static public Regex ClipStringKnife(string mainKnife, string viceKnife)
        {
            return new Regex(FinalKnife(mainKnife, viceKnife));
        }
        static public Regex ClipStringKnife(string onlyKnife)
        {
            return new Regex(KnifeStringChange(onlyKnife.ToLower()));
        }
        /// <summary>
        /// 用< >括號包住主目標
        /// </summary>
        /// <param name="_mainKnife">主目標</param>
        /// <returns></returns>
        static string MainKnife(string _mainKnife)
        {
            return $@"(\<{KnifeStringChange(_mainKnife)}\>)";
        }
        /// <summary>
        /// 用大括號包住副要目標
        /// </summary>
        /// <param name="viceKnife">副要目標</param>
        /// <returns></returns>
        static string ViceKnife(string viceKnife)
        {
            return @"(\{" + KnifeStringChange(viceKnife) + @"\})?";
        }
        /// <summary>
        /// 綜合結果
        /// </summary>
        /// <param name="mainKnife">主目標，已完成</param>
        /// <param name="viceKnife">副要目標，已完成</param>
        /// <returns></returns>
        static string FinalKnife(string mainKnife, string[] viceKnife)
        {
            string result = MainKnife(mainKnife.ToLower());
            for (int i = 0; i < viceKnife.Length; i++) result += ViceKnife(viceKnife[i].ToLower());
            return result;
        }
        static string FinalKnife(string mainKnife, string viceKnife)
        {
            string result = MainKnife(mainKnife.ToLower());
            result += ViceKnife(viceKnife.ToLower());
            return result;
        }
        #endregion
        /// <summary>
        /// position,scale,rotation都是一樣格式
        /// </summary>
        static string numberKnife { get { return @"(\-?\d+)"; } }
        static string psrValueKnife { get { return $@"([xyz])\:{numberKnife}"; } }
        static string psrKnife { get { return $@"x\:{numberKnife},y\:{numberKnife}(,z\:{numberKnife})?"; } }
        static string psrKnife2 { get { return @"\{"+psrKnife+@"\}"; } }
        static string transformKnife { get { return @$"(p:{psrKnife2})?" + @$"(,s:{psrKnife2})?" + @$"(,r:{psrKnife2})?"; } }
        static string imgKnife { get { return @"\$?!?\d+\.img"; } }
        static string compoundWordKnife { get { return @"(.)\{c\:([^\{\}]+)\}"; } }
        static string KnifeStringChange(string input)
        {
            if (input == "img") return imgKnife;
            else if (input == "pos") return psrKnife;
            else if (input == "tra") return transformKnife;
            else if (input == "psrvalue") return psrValueKnife;
            else if (input == "number") return numberKnife;
            else if (input == "wordc") return compoundWordKnife;
            else return @".*";
            
        }
    }
    static class SystemClass
    {
        /// <summary>
        /// 陣列加項
        /// </summary>
        /// <typeparam name="T">陣列類型</typeparam>
        /// <param name="oldArray">舊陣列</param>
        /// <param name="newItem">要加的新項</param>
        /// <returns></returns>
        static public void ArrayAdd<T>(ref T[] oldArray, T newItem)
        {
            int oldLen = oldArray.Length;
            T[] newArray = new T[oldLen + 1];
            for (int i = 0; i < oldLen; i++) newArray[i] = oldArray[i];
            newArray[oldLen] = newItem;
            oldArray = newArray;
        }
        static public void ArrayRemove<T>(ref T[] oldArray, int _index)
        {
            T[] newArray = new T[oldArray.Length - 1];
            for (int i = 0; i < newArray.Length; i++) newArray[i] = oldArray[i + (_index <= i ? 1 : 0)];
            oldArray = newArray;
        }
        static public int ArrayIndex<T>(T[] oldArray, T findItem)
        {
            int resultInt = -1;
            for (int i = 0; i < oldArray.Length; i++) if (oldArray[i].Equals(findItem)) resultInt = i;
            return resultInt;
        }
    }
}
