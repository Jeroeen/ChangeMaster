namespace Assets.Scripts.Json.JsonItems
{
    public struct Priority
    {
        public string PriorityText;
        public int PriorityNumber;
    }

    public class PriorityList
    {
        public Priority[] Priorities;

        public PriorityList(){}

        public PriorityList(Priority[] Information)
        {
            Priorities = Information;
        }

    }
}