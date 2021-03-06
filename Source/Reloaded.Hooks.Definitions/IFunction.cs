﻿using System;
using System.Collections.Generic;
using System.Text;
using Reloaded.Hooks.Definitions.Enums;

namespace Reloaded.Hooks.Definitions
{
    public interface IFunction<TFunction>
    {
        /// <summary>
        /// Address of the function in memory.
        /// </summary>
        long Address { get; }

        /// <summary>
        /// Provides an interface to the hooks library.
        /// </summary>
        IReloadedHooks Hooks { get; }

        /// <summary>
        /// Creates a hook for a function at a given address.
        /// </summary>
        /// <param name="function">The function to detour the original function to.</param>
        /// <param name="minHookLength">Optional explicit length of hook. Use only in rare cases where auto-length check overflows a jmp/call opcode.</param>
        IHook<TFunction> Hook(TFunction function, int minHookLength = -1);

        /// <summary>
        /// Allows you to call this function as if it were a X86 CDECL/X64 Microsoft function.
        /// </summary>
        /// <param name="wrapperAddress">
        ///     Native address of the wrapper used to call the original function.
        ///     If the original function is X86 CDECL/X64 Microsoft, the wrapper address equals the function address.
        /// </param>
        /// <remarks>The return value of this function is cached. Multiple calls will return same value.</remarks>
        TFunction GetWrapper(out IntPtr wrapperAddress);

        /// <summary>
        /// Allows you to call this function as if it were a X86 CDECL/X64 Microsoft function.
        /// </summary>
        /// <remarks>The return value of this function is cached. Multiple calls will return same value.</remarks>
        TFunction GetWrapper();

        /// <summary>
        /// Creates a cheat engine style hook, replacing instruction(s) with a JMP to a user provided set of ASM instructions (and optionally the original ones).
        /// </summary>
        /// <param name="asmCode">
        ///     The assembly code to execute, in FASM syntax.
        ///     (Should start with use32/use64)
        /// </param>
        /// <param name="behaviour">Defines what should be done with the original code that was replaced with the JMP instruction.</param>
        /// <param name="hookLength">Optional explicit length of hook. Use only in rare cases where auto-length check overflows a jmp/call opcode.</param>
        IAsmHook MakeAsmHook(string[] asmCode, AsmHookBehaviour behaviour = AsmHookBehaviour.ExecuteFirst, int hookLength = -1);

        /// <summary>
        /// Creates a cheat engine style hook, replacing instruction(s) with a JMP to a user provided set of ASM instructions (and optionally the original ones).
        /// </summary>
        /// <param name="asmCode">The assembly code to execute, precompiled.</param>
        /// <param name="behaviour">Defines what should be done with the original code that was replaced with the JMP instruction.</param>
        /// <param name="hookLength">Optional explicit length of hook. Use only in rare cases where auto-length check overflows a jmp/call opcode.</param>
        IAsmHook MakeAsmHook(byte[] asmCode, AsmHookBehaviour behaviour = AsmHookBehaviour.ExecuteFirst, int hookLength = -1);
    }
}
