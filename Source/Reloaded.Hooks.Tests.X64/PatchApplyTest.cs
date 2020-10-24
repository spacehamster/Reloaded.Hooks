using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X86;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Xunit;
using static Reloaded.Hooks.Tests.Shared.Macros.Macros;
using FunctionAttribute = Reloaded.Hooks.Definitions.X64.FunctionAttribute;

namespace Reloaded.Hooks.Tests.X64
{
    public class PatchApplyTest : IDisposable
    {
        private Assembler.Assembler _assembler = new Assembler.Assembler();
        private Memory.Sources.Memory _memory = new Memory.Sources.Memory();
        private List<IntPtr> _nativeFunctions = new List<IntPtr>();

        public PatchApplyTest()
        {
            for(int i = 0; i < 1000; i++)
            {
                BuildNativeFunction();
            }
        }

        private void BuildNativeFunction()
        {
            int wordSize = IntPtr.Size;
            string[] addFunction = new string[]
            {
                $"{_use32}",
                $"push {_ebp}",
                $"mov {_ebp}, {_esp}",

                $"mov {_eax}, [{_ebp} + {wordSize * 2}]", // Left Parameter
                $"mov {_ecx}, [{_ebp} + {wordSize * 3}]", // Right Parameter
                $"add {_eax}, {_ecx}",

                $"pop {_ebp}",
                $"ret"
            };

            var result = _assembler.Assemble(addFunction);
            var func = _memory.Allocate(result.Length);
            _memory.WriteRaw(func, result);
            _nativeFunctions.Add(func);
        }
        public void Dispose()
        {
            foreach(var func in _nativeFunctions)
            {
                _memory.Free(func);
            }
            _assembler.Dispose();
        }

        [Fact]
        public void TestAsmHooks()
        {
            List<object> hooks = new List<object>();
            var replacementFuncWrapper = new Hooks.X64.ReverseWrapper<ReplacementFuncDelegate>(ReplacementFunc);
            for(int i = 0; i < _nativeFunctions.Count; i++)
            {
                var nativeFunc = _nativeFunctions[i];
                string[] jumpToReplacement =
                {
                    $"{_use32}",
                    $"mov rax, {replacementFuncWrapper.WrapperPointer}",
                    $"jmp rax"
                };

                var hook = ReloadedHooks.Instance.CreateAsmHook(jumpToReplacement, (long)nativeFunc, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
                hooks.Add(hook);
            }
        }

        [Definitions.X64.Function(Definitions.X64.CallingConventions.Microsoft)]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ReplacementFuncDelegate();

        public static void ReplacementFunc()
        {

        }
    }
}
