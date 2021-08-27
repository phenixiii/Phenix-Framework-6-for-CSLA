using System;

namespace Phenix.Test.使用指南._11._2._2
{
  //* 作为复制的数据源
  /// <summary>
  /// User_Source
  /// </summary>
  [Serializable]
  public class User_Source : User<User_Source>
  {
    private User_Source()
    {
      //禁止添加代码
    }
  }
}
