using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Console;

// ReSharper disable LocalizableElement

namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Dr Ecco", "Zoranic Monopoles",
            "https://books.google.com/books/about/Doctor_Ecco_s_Cyberpuzzles_36_Puzzles_fo.html?id=Y0JXuG3ZDgQC")]
        public class ZoranicMonopoles : IChallenge
        {
            const int MaxMonos = 100;
            // ReSharper disable once UnusedParameter.Local
            public void Solve()
            {
                var iRoomCount = 3;
                var assignments = MonosInRooms(iRoomCount);
                WriteLine($"\nMax Packing in {iRoomCount} rooms:");
                WriteRoomAssignments(assignments);
            }

            private static void WriteRoomAssignments(List<List<int>[]> assignments)
            {
                for (var iPacking = 0; iPacking < assignments.Count; iPacking++)
                {
                    WriteLine($"Packing {iPacking}:");
                    var assignment = assignments[iPacking];
                    WritePacking(assignment);
                }
            }

            private static void WritePacking(List<int>[] assignment)
            {
                for (var iRoom = 0; iRoom < assignment.Length; iRoom++)
                {
                    var fFirstTime = true;
                    var monoList = assignment[iRoom];
                    Write($"Room {iRoom}: ");
                    foreach (var iMono in monoList)
                    {
                        if (!fFirstTime)
                        {
                            Write(' ');
                        }

                        fFirstTime = false;
                        Write(iMono);
                    }

                    WriteLine("");
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>	Determine how many monos we can fit in a number of rooms. </summary>
            ///
            /// <remarks>
            /// This returns a list of room assignments.  Each room assignment is a list with one element for
            /// each room.  Each of those elements is a list of the monopoles that can be placed in that
            /// room.  Darrell Plank, 6/10/2020.
            /// </remarks>
            ///
            /// <param name="cRooms">	The count of rooms. </param>
            ///
            /// <returns>	A List of an array of lists.   </returns>
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            private List<List<int>[]> MonosInRooms(int cRooms)
            {
                var ra = new RoomAssignments(cRooms);
                var (assignments, _) = TryPlace(1, 0, ra, 1);
                return assignments;
            }

            private static (List<List<int>[]> assignments, int maxMono) TryPlace(int monopole, int room, RoomAssignments ra, int minMonos)
            {
                if (!ra.Place(monopole, room))
                {
                    return (new List<List<int>[]>() {ra.CloneAssignments()}, ra.NextUnassigned - 1);
                }

                var ret = new List<List<int>[]>();

                // Good - we were able to place mono in requested room.  Go on and try to place the next monopole
                // in each valid room.
                var nextMono = ra.NextUnassigned;

                var fPlacedInEmptyRoom = false;
                for (var iRoom = 0; iRoom < ra.RoomCount; iRoom++)
                {
                    var isEmptyRoom = ra.MonosInRoom(iRoom).Count == 0;
                    if (fPlacedInEmptyRoom && isEmptyRoom)
                    {
                        // No sense in trying to place the monopole in two separate empty rooms
                        continue;
                    }

                    fPlacedInEmptyRoom = fPlacedInEmptyRoom || isEmptyRoom;
                    var (assignments, count) = TryPlace(nextMono, iRoom, ra, minMonos);
                    if (count > minMonos)
                    {

                        ret = assignments;
                        minMonos = count;
                    }
                    else if (count == minMonos && count >= nextMono)
                    {
                        ret.AddRange(assignments);
                    }

                    // Only remove if we successfully placed
                    if (count >= nextMono)
                    {
                        ra.Unplace(nextMono, iRoom);
                    }
                }

                return (ret, minMonos);
            }

            public string RetrieveSampleInput() => null;

            public string RetrieveSampleOutput() => null;

            private class RoomAssignments
            {
                // A list of the monopoles in each room.
                List<int>[] _rooms;

                // We might as well assume that M0 goes in room 0.  After that I'm not sure what direct conclusions
                // we can make.  Should M1 go in the same room or a different room?  I suppose that if you've got
                // e empty rooms then the first one to occupy one of those empty rooms may as well go in the next
                // one.  It's a generalization of M0 occupying room 0 and means that when we backtrack over 
                // the placement of a monopole in an empty room we needn't bother trying to place it in another
                // empty room.  We need to keep a list for every monopole of the rooms it's excluded from.  Thus,
                // we have an array of the number of ways a particular monopole can be excluded from a room.  So
                // 5 can be excluded twice from a room with 1,2,3 and 4 in it - once by 1+4 and once by 2+3.  By
                // keeping a count we can "unplace" a monopole in a room and remove it's prohibions easily.  Thus
                // _unavailable[mono, room] is the count of ways that mono is excluded from room.
                private readonly int[,] _exclusions;

                public int RoomCount => _rooms.Length;
                public int NextUnassigned { get; private set; }

                public RoomAssignments(int cRooms)
                {
                    _rooms = new List<int>[cRooms];
                    for (var iRoom = 0; iRoom < cRooms; iRoom++)
                    {
                        _rooms[iRoom] = new List<int>();
                    }
                    _exclusions = new int[MaxMonos, cRooms];
                }

                public List<int>[] CloneAssignments()
                {
                    var clone = new List<int>[RoomCount];
                    for (int iRoom = 0; iRoom < RoomCount; iRoom++)
                    {
                        clone[iRoom] = _rooms[iRoom].ToList();
                    }

                    return clone;
                }

                public List<int>[] CurrentRoomAssignment() => _rooms;

                public List<int> MonosInRoom(int room) => _rooms[room];

                public bool Place(int monopole, int room)
                {
                    // Check to see if we can put this mono in this room
                    if (_exclusions[monopole, room] != 0)
                    {
                        return false;
                    }

                    // Take care that this monopole excludes future illegal placements
                    foreach (var mono in _rooms[room].TakeWhile(mono => mono + monopole < MaxMonos))
                    {
                        _exclusions[mono + monopole, room]++;
                    }

                    // Okay - we can put the monopole in this room
                    _rooms[room].Add(monopole);

                    NextUnassigned = monopole + 1;
                    return true;
                }

                public void Unplace(int monopole, int room)
                {
                    _rooms[room].Remove(monopole);
                    foreach (var mono in _rooms[room].TakeWhile(mono => mono + monopole < MaxMonos))
                    {
                        _exclusions[mono + monopole, room]--;
                    }

                    NextUnassigned = monopole;
                }
            }
        }
    }
}