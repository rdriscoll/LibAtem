using LibAtem.Commands;
using LibAtem.Commands.Audio;
using LibAtem.Common;

namespace LibAtem.MacroOperations.Audio
{
    [MacroOperation(MacroOperationType.AudioMixerMasterOutResetPeaks, 4), NoMacroFields]
    public class AudioMixerMasterOutResetPeaksMacroOp : MacroOpBase
    {
        public override ICommand ToCommand()
        {
            return new AudioMixerResetPeaksCommand
            {
                Mask = AudioMixerResetPeaksCommand.MaskFlags.Master,
                Master = true,
            };
        }
    }
}