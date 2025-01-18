using System;
using System.Collections.Generic;
namespace Projects.Demo0.Core
{
public struct DishClueState : IEquatable<DishClueState>
{
	public bool Correct;
	public bool Polluted;
	public DishClueState(bool correct, bool polluted) =>
		(Correct, Polluted) = (correct, polluted);
	public bool Equals(DishClueState other) =>
		Correct == other.Correct && Polluted == other.Polluted;
}
public class DishClueStateDoc
{
	public DishClueState State;
	public List<Cmd> InteractCmdList = new();
}
}