﻿using System;

namespace Reloaded.Hooks.Definitions
{
    public interface IReverseWrapper<TFunction>
    {

        /// <summary> Copy of C# function behind the pointer. </summary>
        TFunction CSharpFunction { get; }

        /// <summary> Pointer to the function that gets executed inside the wrapper, either native or C#. </summary>
        IntPtr NativeFunctionPtr { get; }

        /// <summary> A pointer to our wrapper, which calls the internal method as if it were to be of another convention. </summary>
        IntPtr WrapperPointer { get; }
    }
}