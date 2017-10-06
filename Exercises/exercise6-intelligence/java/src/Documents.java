import java.util.ArrayList;
import java.util.List;

public class Documents {
    private List<Document> documents;

    public Documents() {
        this.documents = new ArrayList<Document>();
    }

    public List<Document> getDocuments() {
        return this.documents;
    }

    public void add(String id, String text) {
        this.documents.add (new Document (id, text));
    }
}
