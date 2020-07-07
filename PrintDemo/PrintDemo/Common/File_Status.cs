using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintDemo.Common
{
    #region 文件状态枚举

    /// <summary>
    /// 文件状态枚举
    /// </summary>
    public enum File_Status
    {
        // 文件不存在
        NonExisting = 0,

        // 文件未被占用
        Unlocked = 1,

        // 文件被占用
        Locked = 2

    }

    #endregion
}
