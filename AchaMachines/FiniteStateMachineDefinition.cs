// Copyright (c) 2009 River Champeimont
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
	/*
	This class implements a Finite State Machine "definition".
	By definition we mean that it is not a running machine, but just
	the list of transitions and states, which contain the information
	that the quintuple below contains.

	A deterministic finite state machine is a quintuple (Σ,S,s0,δ,F), where:
	Σ is the alphabet (a finite set)
	S is the set of states (a finite set)
	s0 is the initial state, an element of S
	δ is the state-transition function: S x Σ -> S
	F is the set of final states, a (possibly empty) subset of S
	  
	See http://en.wikipedia.org/w/index.php?title=Finite_state_machine for more infos
	on finite state machines.
	*/
	public class FiniteStateMachineDefinition<TAlphabet,TState>
	{
		
		internal FiniteStateMachineDefinitionById definitionById;
		
		/* Alphabet. */
		private TAlphabet[] alphabet; // letter id -> letter
		private Dictionary<TAlphabet, int> alphabetIds; // letter -> letter id
		/* Letter ids are integers starting at 0 and in the order.
		 * They are the positions in the transition matrix, ie
		 * transitionMatrix[stateId, letterId] is the state reached when
		 * reading letter with id letterId.
		 */
		public TAlphabet LetterOfLetterId(int letterId) {
			return alphabet[letterId];
		}
		public int LetterIdOfLetter(TAlphabet letter) {
			return alphabetIds[letter];
		}
		public int AlphabetSize {
			get {
				return definitionById.AlphabetSize;
			}
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
		public int TransitionById(int stateId, int letterId) {
			return definitionById.Transition(stateId, letterId);
		}
		/* Same thing using TState and TAlphabet instead of ids. */
		public TState Transition(TState state, TAlphabet letter) {
			return StateOfStateId(TransitionById(StateIdOfState(state), LetterIdOfLetter(letter)));
		}
		
		
		
		/* Final states. */
		public bool IsFinalById(int stateId) {
			return definitionById.IsFinal(stateId);
		}
		public bool IsFinal(TState state) {
			return IsFinalById(StateIdOfState(state));
		}

		
		
		
		/* This constructor creates a finite state machine definition. The arguments are:
		 * - the alphabet (an array without duplicates)
		 * - the states (an array without duplicates)
		 * - an initial state id (must be a member of the sates)
		 * - a transition matrix, with lines being states, columns beeing letter and content being the new states,
		 *   ie newMachinesTransitions[s1_id,a_id] = s2_id means that when the machine is in state newMachineStates[s1_id]
		 *   and reads letter newMachineAlphabet[a_id], it goes in state newMachineStates[s2].
		 * - an vector of booleans, as many as states, which say if the state is final or not
		 * 
		 * About FrozenVector and FrozenMatrix:
		 * This object requires that you use FrozenVector and FrozenMatrix for
		 * the transitions matrix and the vector of final states.
		 * As FrozenVector and FrozenMatrix are read-only, they can safely be
		 * shared accross machines definitions. For convenience, a overrided
		 * constructor is provided, that takes usual arrays, and converts them
		 * to FrozenMatrix/Vector.
		 */
		// IDS OR CUSTOM TYPES?
		// States and letters have a double representation. They can bee seen as
		// integer ids (positions in the matrix, newMachineBlankSymbolId, newMachineInitialStateId)
		// or as TState and TAlphabet (types you choose). The mapping between this 2 representations
		// is acomplished using newMachineTapeAlphabet and newMachineStates. It is not mandatory
		// to use this 2 representations. You may be happy with just integers. In that case,
		// pass <int,int> as <TAlphabet,TState> and null as newMachineTapeAlphabet and newMachineStates.
		// The number of states and letters will then be computer using matrix lengths.
		// You can have a mix of the 2, for example state as ids only but letters as cutom type.
		public FiniteStateMachineDefinition(ICollection<TAlphabet> newMachineAlphabet,
		                                    ICollection<TState> newMachineStates,
		                                    int newMachineInitialStateId,
		                                    FrozenMatrix<int> newMachineTransitions,
		                                    FrozenVector<bool> newMachineFinalStates) {
			
			int alphabetSize;
			/* Store alphabet and check it is correct (ie no duplicates). */
			if (newMachineAlphabet != null) {
				alphabet = new TAlphabet[newMachineAlphabet.Count];
				newMachineAlphabet.CopyTo(alphabet, 0);
				alphabetIds = new Dictionary<TAlphabet,int>();
				int i = 0;
				foreach (TAlphabet a in newMachineAlphabet)
				{
					if (alphabetIds.ContainsKey(a)) {
						throw new FiniteStateMachineException("Duplicated letter in newMachineAlphabet : " + a);
					}
					alphabetIds.Add(a, i);
					i++;
				}
				// newMachineAlphabet array has authority for giving the number of letters
				alphabetSize = alphabet.Length;
			} else {
				if (typeof(TAlphabet) != typeof(int)) {
					throw new FiniteStateMachineException("You passed null as newMachineAlphabet, so I deduce that you want to identify"
					                                      + " letters with ids instead of TAlphabet type, but did not choose int as TAlphabet"
					                                      + " so this is inconsistant.");
				}
				// the matrix has authority for giving the number of letters
				alphabetSize = newMachineTransitions.GetLength(1);
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
			definitionById = new FiniteStateMachineDefinitionById(alphabetSize,
			                                                      numberOfStates,
			                                                      newMachineInitialStateId,
			                                                      newMachineTransitions,
			                                                      newMachineFinalStates);
			
		}
		
		
		
		/* Creates an execution of this machine definition, starting at the initial state.
		 * This is equivalent to calling:
		 * new FiniteStateMachineExecution<TAlphabet,TState>(this)
		 */
		public FiniteStateMachineExecution<TAlphabet,TState> NewExecution() {
			return new FiniteStateMachineExecution<TAlphabet,TState>(this);
		}
		
		
		

		
		
		
		/* Creates a new execution that reads word, and return the reached state. */
		public int ReadAndReturnStateId(IEnumerable<TAlphabet> word) {
			var fsmExecution = NewExecution();
			fsmExecution.Read(word);
			return fsmExecution.StateId;
		}
		public TState ReadAndReturnState(IEnumerable<TAlphabet> word) {
			return StateOfStateId(ReadAndReturnStateId(word));
		}
		public int ReadByIdAndReturnStateId(IEnumerable<int> word) {
			var fsmExecution = NewExecution();
			fsmExecution.ReadById(word);
			return fsmExecution.StateId;
		}
		public TState ReadByIdAndReturnState(IEnumerable<int> word) {
			return StateOfStateId(ReadByIdAndReturnStateId(word));
		}
		
		
		
		
		/* Tell if the word is accepted by the machine.
		 * (ie. if, after starting at initial state and reading the word, the machines arrives in a final state)
		 */
		public bool Accepts(IEnumerable<TAlphabet> word) {
			return IsFinalById(ReadAndReturnStateId(word));
		}
		public bool AcceptsById(IEnumerable<int> word) {
			return IsFinalById(ReadByIdAndReturnStateId(word));
		}
			
		// This is just a shortcut.
		// Calling this ctor(a, s, i, t, f) will simply call:
		// ctor(a, s, i, new FrozenMatrix<int>(t), new FrozenVector<bool>(f)
		public FiniteStateMachineDefinition(TAlphabet[] newMachineAlphabet,
		                                    TState[] newMachineStates,
		                                    int newMachineInitialStateId,
		                                    int[,] newMachineTransitions,
		                                    bool[] newMachineFinalStates)
			: this(newMachineAlphabet,
			       newMachineStates,
			       newMachineInitialStateId,
			       new FrozenMatrix<int>(newMachineTransitions),
			       new FrozenVector<bool>(newMachineFinalStates))
		{
		}
		
		public TuringMachineDefinition<int,int> ToTuringMachineDefinitionById() {
			int newTapeAlphabetSize = AlphabetSize + 1; // the FSM alphabet + the blank symbol
			int blank = AlphabetSize;
			int[,] newTransitions = new int[NumberOfStates, newTapeAlphabetSize*3];
			for (int l=0; l<AlphabetSize; l++) {
				for (int s=0; s<NumberOfStates; s++) {
					newTransitions[s, 3*l] = TransitionById(s, l);
					newTransitions[s, 3*l+1] = l; // Do not change tape content.
					newTransitions[s, 3*l+2] = 1; // Go to the right.
				}
			}
			for (int s=0; s<NumberOfStates; s++) {
				newTransitions[s, 3*blank] = IsFinalById(s) ? -2 : -1;
				newTransitions[s, 3*blank+1] = blank;
				newTransitions[s, 3*blank+2] = 1;
			}
			var tm = new TuringMachineDefinition<int,int>(null,
			                                              blank,
			                                              null,
			                                              null,
			                                              InitialStateId,
			                                              new FrozenMatrix<int>(newTransitions));
			return tm;
		}
		
		
	}
}
