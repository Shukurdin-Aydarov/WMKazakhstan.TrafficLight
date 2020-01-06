using WMKazakhstan.TrafficLight.Core.Exceptions;

namespace WMKazakhstan.TrafficLight.Core
{
    public static class Throws
    {
        public static void NoSolutionFound()
            => throw new CoreException("No solution found");

        public static void DuplicateObservation()
            => throw new CoreException("Duplicate observation");

        public static void SequenceNotFound()
            => throw new CoreException("The sequence isn't found");

        public static void NotEnoughData()
            => throw new CoreException("There isn't enough data");

        public static void RedShouldBeTheLast()
            => throw new CoreException("The red observation should be the last");
    }
}
