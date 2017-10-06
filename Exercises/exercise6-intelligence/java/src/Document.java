
public class Document {
    private String id;
    private String text;

    public String getId() {
        return this.id;
    }

    public String getText() {
        return this.text;
    }

    public Document(String id, String text){
        this.id = id;
        this.text = text;
    }
}

