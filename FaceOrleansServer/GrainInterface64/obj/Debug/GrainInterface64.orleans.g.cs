#if !EXCLUDE_CODEGEN
#pragma warning disable 162
#pragma warning disable 219
#pragma warning disable 414
#pragma warning disable 649
#pragma warning disable 693
#pragma warning disable 1591
#pragma warning disable 1998
[assembly: global::System.CodeDom.Compiler.GeneratedCodeAttribute("Orleans-CodeGenerator", "1.5.3.0")]
[assembly: global::Orleans.CodeGeneration.OrleansCodeGenerationTargetAttribute("GrainInterface64, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
namespace GrainInterface64
{
    using global::Orleans.Async;
    using global::Orleans;
    using global::System.Reflection;

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Orleans-CodeGenerator", "1.5.3.0"), global::System.SerializableAttribute, global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute, global::Orleans.CodeGeneration.GrainReferenceAttribute(typeof (global::GrainInterface64.IFaceCompare))]
    internal class OrleansCodeGenFaceCompareReference : global::Orleans.Runtime.GrainReference, global::GrainInterface64.IFaceCompare
    {
        protected @OrleansCodeGenFaceCompareReference(global::Orleans.Runtime.GrainReference @other): base (@other)
        {
        }

        protected @OrleansCodeGenFaceCompareReference(global::System.Runtime.Serialization.SerializationInfo @info, global::System.Runtime.Serialization.StreamingContext @context): base (@info, @context)
        {
        }

        protected override global::System.Int32 InterfaceId
        {
            get
            {
                return 1032600756;
            }
        }

        protected override global::System.UInt16 InterfaceVersion
        {
            get
            {
                return 1;
            }
        }

        public override global::System.String InterfaceName
        {
            get
            {
                return "global::GrainInterface64.IFaceCompare";
            }
        }

        public override global::System.Boolean @IsCompatible(global::System.Int32 @interfaceId)
        {
            return @interfaceId == 1032600756;
        }

        protected override global::System.String @GetMethodName(global::System.Int32 @interfaceId, global::System.Int32 @methodId)
        {
            switch (@interfaceId)
            {
                case 1032600756:
                    switch (@methodId)
                    {
                        case -510773597:
                            return "SayHello";
                        default:
                            throw new global::System.NotImplementedException("interfaceId=" + 1032600756 + ",methodId=" + @methodId);
                    }

                default:
                    throw new global::System.NotImplementedException("interfaceId=" + @interfaceId);
            }
        }

        public global::System.Threading.Tasks.Task<global::System.String> @SayHello(global::System.String @msg)
        {
            return base.@InvokeMethodAsync<global::System.String>(-510773597, new global::System.Object[]{@msg});
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Orleans-CodeGenerator", "1.5.3.0"), global::Orleans.CodeGeneration.MethodInvokerAttribute(typeof (global::GrainInterface64.IFaceCompare), 1032600756), global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
    internal class OrleansCodeGenFaceCompareMethodInvoker : global::Orleans.CodeGeneration.IGrainMethodInvoker
    {
        public global::System.Threading.Tasks.Task<global::System.Object> @Invoke(global::Orleans.Runtime.IAddressable @grain, global::Orleans.CodeGeneration.InvokeMethodRequest @request)
        {
            global::System.Int32 interfaceId = @request.@InterfaceId;
            global::System.Int32 methodId = @request.@MethodId;
            global::System.Object[] arguments = @request.@Arguments;
            if (@grain == null)
                throw new global::System.ArgumentNullException("grain");
            switch (interfaceId)
            {
                case 1032600756:
                    switch (methodId)
                    {
                        case -510773597:
                            return ((global::GrainInterface64.IFaceCompare)@grain).@SayHello((global::System.String)arguments[0]).@Box();
                        default:
                            throw new global::System.NotImplementedException("interfaceId=" + 1032600756 + ",methodId=" + methodId);
                    }

                default:
                    throw new global::System.NotImplementedException("interfaceId=" + interfaceId);
            }
        }

        public global::System.Int32 InterfaceId
        {
            get
            {
                return 1032600756;
            }
        }

        public global::System.UInt16 InterfaceVersion
        {
            get
            {
                return 1;
            }
        }
    }
}
#pragma warning restore 162
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 649
#pragma warning restore 693
#pragma warning restore 1591
#pragma warning restore 1998
#endif
