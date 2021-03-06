using LibAtem.Commands;
using LibAtem.Commands.DownstreamKey;
using LibAtem.Common;
using LibAtem.Serialization;

namespace LibAtem.MacroOperations.DownStreamKey
{
    [MacroOperation(MacroOperationType.DownstreamKeyCutInput, 8)]
    public class DownstreamKeyCutInputMacroOp : DownstreamKeyMacroOpBase
    {
        [Serialize(6), Enum16]
        [MacroField("Input")]
        public VideoSource Input { get; set; }

        public override ICommand ToCommand()
        {
            return new DownstreamKeyCutSourceSetCommand()
            {
                Index = KeyIndex,
                Source = Input,
            };
        }
    }
}