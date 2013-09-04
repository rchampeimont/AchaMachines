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
using AchaMachines;

// IMPLEMENTATION STATUS: STABLE (you can assume the interface will not change,
// or will change but keep backward compatibility).

namespace AchaMachines
{
	// Our formal definition of a Turing machine is a tuple M=(Γ,b,Σ,Q,q_0,δ):
	//  * Γ is a finite set of the tape symbols, with at least one letter (the blank symbol)
	//  * b ∈ Γ is the blank symbol (the only symbol allowed to occur on the tape infinitely often at any step during the computation)
	//  * Σ ⊆ Γ\{b} is the alphabet of words that are given to the machine
	//  * Q is a finite set of states, with at least one state
	//  * q_0  Q is the initial state
	//  * δ: Q x Γ  -> (Q ∪ {-1,-2}) x Γ x {-1,0,1} is a function called the transition function
	//       (s, l) -> (s',            l', d')
	//    with s and l beeing the read state and letter, and s', l', and d'
	//    beeing the new state, new letter and direction.
	//    New states -1 and -2 are special cases, they mean halt+reject and halt+accept.
	//    Directions -1, 0, 1 mean "left", "don't move", "right".
	public class TuringMachineDefinition<TAlphabet,TState>
	{
		
				
		internal TuringMachineDefinitionById definitionById;
		
		/* Alphabet. */
		private TAlphabet[] tapeAlphabet; // letter id -> letter
		private Dictionary<TAlphabet, int> tapeAlphabetIds; // letter -> letter id
		/* Letter ids are integers starting at 0 and in the order.
		 * They are the positions in the transition matrix, ie
		 * transitionMatrix[stateId, letterId] is the state reached when
		 * reading letter with id letterId.
		 */
		public TAlphabet LetterOfLetterId(int letterId) {
			return tapeAlphabet[letterId];
		}
		public int LetterIdOfLetter(TAlphabet letter) {
			return tapeAlphabetIds[letter];
		}
		public int TapeAlphabetSize {
			get {
				return definitionById.TapeAlphabetSize;
			}
		}
		
		public int BlankId {
			get {
				return definitionById.Blank;
			}
		}
		public TAlphabet Blank {
			get {
				return LetterOfLetterId(BlankId);
			}
		}
		
		public bool InputAlphabetById(int letterId) {
			return definitionById.InputAlphabet(letterId);
		}
		public bool InputAlphabet(TAlphabet letter) {
			return definitionById.InputAlphabet(LetterIdOfLetter(letter));
		}
		
		
		/* State. */
		private TState[] states; // state id -> state
		private Dictionary<TState, int> stateIds; // state -> state id
		/* State ids are the same thing as letter ids, but for states. */
		public TState StateOfStateId(int stateId) {
			return states[stateId];
		}
		public int StateIdOfState(TState state) {
			return stateIds[state];
		}
		public int NumberOfStates {
			get {
				return definitionById.NumberOfStates;
			}
		}
		
		
		
		/* Initial. */
		public int InitialStateId {
			get {
				return definitionById.InitialState;
			}
		}
		public TState InitialState {
			get {
				return StateOfStateId(InitialStateId);
			}
		}
		
		
		
		/* Transition matrix. */
		public int TransitionMatrixById(int stateId, int j) {
			return definitionById.TransitionMatrix(stateId, j);
		}
		
		
		
		// This constructor creates a Turing machine definition. The arguments are:
		// (Note: We use "symbol" and "letter" as synonims.)
		//
		// IDS OR CUSTOM TYPES?
		// States and letters have a double representation. They can bee seen as
		// integer ids (positions in the matrix, newMachineBlankSymbolId, newMachineInitialStateId)
		// or as TState and TAlphabet (types you choose). The mapping between this 2 representations
		// is acomplished using newMachineTapeAlphabet and newMachineStates. It is not mandatory
		// to use this 2 representations. You may be happy with just integers. In that case,
		// pass <int,int> as <TAlphabet,TState> and null as newMachineTapeAlphabet and newMachineStates.
		// The number of states and letters will then be computer using matrix lengths.
		// You can have a mix of the 2, for example state as ids only but letters as cutom type.
		//
		// - the tape alphabet (length >= 1)
		// - a blank symbol (>= 0, <(alphabet size))
		// - the set of letters that are part of the input alphabet, subset of the tape alphabet,
		//   it is an array of bools of length = alphabet size. The blank symbol
		//   must not be part of the input alphabet. newMachineInputAlphabet[a] contains
		//   true of false whether or not letter a is part of the input alphabet.
		//   If you want the input alphabet to be all the tape alphabet except blank,
		//   then you can pass null as this argument.
		// - states (length >= 1)
		// - an initial state (>= 0, <(number of states))
		// - a transition matrix, with lines being states, 3 columns beeing one letter and content being
		//   the new state, the new letter and the direction.
		//   ie. newMachinesTransitions[s1,3*a] = s2 means that when the machine is in state s1
		//   and reads letter a, it goes in state s2. Special values -1 and -2 respectively mean
		//   reject and accept.
		//   newMachinesTransitions[s1,3*a+1] = a2 means the written letter is a2
		//   newMachinesTransitions[s1,3*a+2] = d means the machine will go in direction d
		//   with 0 beeing "don't move", 1 beeing "go one step right", and 2 beeing "go one step left". 
		//
		// About FrozenVector and FrozenMatrix:
		// This object requires that you use FrozenVector and FrozenMatrix for
		// the transitions matrix and the vector of final states.
		// As FrozenVector and FrozenMatrix are read-only, they can safely be
		// shared accross machines definitions. For convenience, a overrided
		// constructor is provided, that takes usual arrays, and converts them
		// to FrozenMatrix/Vector.
		// 
		public TuringMachineDefinition(ICollection<TAlphabet> newMachineTapeAlphabet,
		                               int newMachineBlankSymbolId,
		                               FrozenVector<bool> newMachineInputAlphabet,
		                               ICollection<TState> newMachineStates,
		                               int newMachineInitialStateId,
		                               FrozenMatrix<int> newMachineTransitions) {
			

						
			int tapeAlphabetSize;
			/* Store alphabet and check it is correct (ie no duplicates). */
			if (newMachineTapeAlphabet != null) {
				tapeAlphabet = new TAlphabet[newMachineTapeAlphabet.Count];
				newMachineTapeAlphabet.CopyTo(tapeAlphabet, 0);
				tapeAlphabetIds = new Dictionary<TAlphabet,int>();
				int i = 0;
				foreach (TAlphabet a in newMachineTapeAlphabet)
				{
					if (tapeAlphabetIds.ContainsKey(a)) {
						throw new FiniteStateMachineException("Duplicated letter in newMachineAlphabet : " + a);
					}
					tapeAlphabetIds.Add(a, i);
					i++;
				}
				// newMachineAlphabet array has authority for giving the number of letters
				tapeAlphabetSize = tapeAlphabet.Length;
			} else {
				if (typeof(TAlphabet) != typeof(int)) {
					throw new FiniteStateMachineException("You passed null as newMachineAlphabet, so I deduce that you want to identify"
					                                      + " letters with ids instead of TAlphabet type, but did not choose int as TAlphabet"
					                                      + " so this is inconsistant.");
				}
				// the matrix has authority for giving the number of letters
				tapeAlphabetSize = newMachineTransitions.GetLength(1)/3;
			}
			
			
			int numberOfStates;
			/* Store states and check it is correct (ie no duplicates). */
			if (newMachineStates != null) {
				states = new TState[newMachineStates.Count];
				newMachineStates.CopyTo(states, 0);
				stateIds = new Dictionary<TState,int>();
				int i = 0;
				foreach (TState s in newMachineStates)
				{
					if (stateIds.ContainsKey(s)) {
						throw new FiniteStateMachineException("Duplicated state in newMachineStates : " + s);
					}
					stateIds.Add(s, i);
					i++;
				}
				// newMachineStates array has auothority for giving the number of letters
				numberOfStates = states.Length;
			} else {
				if (typeof(TState) != typeof(int)) {
					throw new FiniteStateMachineException("You passed null as newMachineStates, so I deduce that you want to identify"
					                                      + " states with ids instead of TState type, but did not choose int as TState"
					                                      + " so this is inconsistant.");
				}
				// the matrix has authority for giving the number of states
				numberOfStates = newMachineTransitions.GetLength(0);
			}
			
			
			/* Now build our internal FiniteStateMachineDefinitionById. */
			definitionById = new TuringMachineDefinitionById(tapeAlphabetSize,
			                                                 newMachineBlankSymbolId,
			                                                 newMachineInputAlphabet,
			                                                 numberOfStates,
			                                                 newMachineInitialStateId,
			                                                 newMachineTransitions);
			
			
			
		}
		
		

		// This constructor creates a machine execution from a machine definition.
		// The created machine starts at the initial state with word as initial content.
		// The word is given as letters ids.
		public TuringMachineExecution<TAlphabet,TState> NewExecutionById(IEnumerable<int> word) {
			var execution = new TuringMachineExecution<TAlphabet,TState>(this);
			
			// Build initial tape.
			foreach (int letterId in word) {
				if (!InputAlphabetById(letterId)) {
					throw new TuringMachineException("Letter " + letterId + " is not in input alphabet.");
				}
			}
			execution.Tape = new LinkedList<int>(word);
			// If empty tape, still create one blank symbol, so that the cursor is on something.
			if (execution.Tape.First == null) {
				execution.Tape.AddLast(BlankId);
			}
			
			// Put cursor on first symbol of the tape.
			execution.Cursor = execution.Tape.First;
			
			// Initialize state.
			execution.StateId = InitialStateId;
			
			return execution;
		}
		// This constructor creates a machine execution from a machine definition.
		// The created machine starts at the initial state with en empty tape
		public TuringMachineExecution<TAlphabet,TState> NewExecutionById() {
			int[] empty = {};
			return NewExecutionById(empty);
		}
		// This constructor creates a machine execution from a machine definition.
		// The created machine starts at the initial state with word as initial content.
		// The word is given as enumerable set of TAlphabet.
		public TuringMachineExecution<TAlphabet,TState> NewExecution(IEnumerable<TAlphabet> word) {
			var wordById = new LinkedList<int>();
			foreach (TAlphabet letter in word) {
				wordById.AddLast(LetterIdOfLetter(letter));
			}
			return NewExecutionById(wordById);
		}
		// This constructor creates a machine execution from a machine definition.
		// The created machine starts at the initial state with en empty tape
		// It is the same as NewExecutionById()
		public TuringMachineExecution<TAlphabet,TState> NewExecution() {
			return NewExecutionById();
		}
		
		
		// Tell if the word is accepted by the machine.
		// ie. if, after starting at initial state and with this word on the tape,
		// tha machine halts in the accepting state
		public bool Accepts(IEnumerable<TAlphabet> word) {
			var execution = NewExecution(word);
			return (execution.WorkUntilHalts() == -2);
		}
		
		public bool AcceptsById(IEnumerable<int> word) {
			var execution = NewExecutionById(word);
			return (execution.WorkUntilHalts() == -2);
		}
		
		

	}
}
