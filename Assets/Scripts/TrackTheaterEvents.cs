using UnityEngine;

public class TrackTheaterEvents : MonoBehaviour
{
    string movieTitle;
    bool ticketBought;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movieTitle = "";
        ticketBought = false;
    }

    public void setMovieTitle(string title) {
        movieTitle = title;
    }

    public string getMovieTitle() {
        return movieTitle;
    }

    public void buyTicket() {
        ticketBought = true;
    }

    public bool isTicketBought() {
        return ticketBought;
    }
}
