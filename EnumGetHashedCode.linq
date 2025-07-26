<Query Kind="Statements" />

AudioSamplingRatesEnum x = AudioSamplingRatesEnum.Rate16KHz;
x.GetHashCode().Dump();

public enum AudioSamplingRatesEnum
{
	Rate8KHz = 8000,
	Rate16KHz = 16000,
	Rate24kHz = 24000,
	Rate44_1kHz = 44100,
	Rate48kHz = 48000
}