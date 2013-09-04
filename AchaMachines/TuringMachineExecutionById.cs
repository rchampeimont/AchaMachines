// Copyright (c) 2009 Raphael Champeimont
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;

// This file contains only internal code.

namespace AchaMachines
{
	internal class TuringMachineExecutionById // TO MAKE INTERNAL
	{
		// Reference to the definition.
		private TuringMachineDefinitionById definition;
		public TuringMachineDefinitionById Definition {
			get {
				return definition;
			}
		}
		
		public LinkedList<int> Tape { get; set; }
		
		public LinkedListNode<int> Cursor { get; set; }
		
		public int State { get; set; }
		
		public bool Halted {
			get {
				return (State < 0);
			}
		}
		
		// Initialize the state and create a 0-length tape.
		internal TuringMachineExecutionById(TuringMachineDefinitionById machineDefinition)
		{
			definition = machineDefinition;
		}
		
		
		// Do one step: change state, write letter, move.
		public void DoOneStep() {
			if (Halted) {
				// If machine has halted, do nothing at all.
				return;
			}
			
			int previousLetter = Cursor.Value;
			int previousState = State;
			
			// Update machine state.
			State = definition.TransitionMatrix(previousState, 3*previousLetter);
			
			// Write new letter.
			Cursor.Value = definition.TransitionMatrix(previousState, 3*previousLetter+1);
			
			// Move.
			int direction = definition.TransitionMatrix(previousState, 3*previousLetter+2);
			switch (direction) {
			case 0:
				// We don't move.
				break;
			case 1:
			{
				// We will move to the right, so future position is next node in linked list.
				LinkedListNode<int> futurePosition = Cursor.Next;
				
				// If we are on the left end of the tape, and we have written a blank
				// on current position, then "cut" this position from the tape.
				// (to save RAM)
				if ((Cursor.Previous == null) && (Cursor.Value == definition.Blank)) {
					Tape.RemoveFirst();
				}
				
				// Now check if the current position is not the right end of the tape,
				// if it is, we must add a piece of tape to the right.
				if (futurePosition == null) {
					Tape.AddLast(definition.Blank);
					futurePosition = Tape.Last;
				}
				
				// Now move.
				Cursor = futurePosition;
			}
				break;
			case -1:
			{
				// We will move to the left, so future position is previous node in linked list.
				LinkedListNode<int> futurePosition = Cursor.Previous;
				
				// If we are on the right end of the tape, and we have written a blank
				// on current position, then "cut" this position from the tape.
				// (to save RAM)
				if ((Cursor.Next == null) && (Cursor.Value == definition.Blank)) {
					Tape.RemoveLast();
				}
				
				// Now check if the current position is not the left end of the tape,
				// if it is, we must add a piece of tape to the left.
				if (futurePosition == null) {
					Tape.AddFirst(definition.Blank);
					futurePosition = Tape.First;
				}
				
				// Now move.
				Cursor = futurePosition;
			}
				break;
			default:
				throw new FiniteStateMachineException("Internal error, direction = " + direction);
			}
		}
		
		// Do n steps. This is equivalent to invoking DoOneStep() n times.
		public void DoManySteps(int n) {
			for (int i=0; i<n && (!Halted); i++) {
				DoOneStep();
			}
		}
		
		// Work until the machine stops. Note that some Turing machines don't halt.
		// Returns -2 if halted in accept state, -1 if halted in reject state.
		public int WorkUntilHalts() {
			while (!Halted) {
				DoOneStep();
			}
			return State;
		}
		
		private string MaybeFullStateAsString(bool full) {
			string s = "";
			LinkedListNode<int> block = Tape.First;
			while (block != null) {
				if (full && (block == Cursor)) {
					s += "[" + State + "]>";
				}
				s += block.Value;
				block = block.Next;
				if (block != null) {
					s += " ";
				}
			}
			return s;
		}
		
		public string FullStateAsString() {
			return MaybeFullStateAsString(true);
		}
		
		public string TapeAsString() {
			return MaybeFullStateAsString(false);
		}
			
	}
}
