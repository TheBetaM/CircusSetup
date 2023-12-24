using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace CircusSetup.Script.LUA
{
    public enum OpCode50 {
        Move, LoadK, LoadBool, LoadNil, GetUpVal,
        GetGlobal, GetTable, SetGlobal, SetUpVal, SetTable,
        NewTable, Self, Add, Sub, Mul,
        Div, Pow, Unm, Not, Concat, 
        Jump, Equal, LessThan, LessEqual, Test,
        Call, TailCall, Return, ForLoop, TForLoop,
        TForPrep, SetList, SetListO, Close, Closure,
    }

    public enum OpCode51 {
        Move, LoadK, LoadBool, LoadNil, GetUpVal,
        GetGlobal, GetTable, SetGlobal, SetUpVal,SetTable,
        NewTable, Self, Add, Sub, Mul,
        Div, Mod, Pow, Unm, Not,
        Len, Concat, Jump, Equal, LessThan,
        LessEqual, Test, TestSet, Call, TailCall,
        Return, ForLoop, ForPrep, TForLoop, SetList,
        Close, Closure, Vararg,
    }

    public enum OpCodeTitans {
        Move, LoadK, LoadBool, LoadNil, GetUpVal,
        GetGlobal, GetTable, SetGlobal, SetUpVal, SetTable,
        NewTable, Self, Add, Sub, Mul,
        Div, Pow, Unm, Not, Concat, 
        Jump, Equal, LessThan, LessEqual, Test,
        Call, TailCall, Return, ForLoop, TForLoop,
        TForPrep, SetList, SetListO, Close, Closure,
        TitansAdd
    }
}