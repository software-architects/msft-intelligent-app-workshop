import java.net.URI;
import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.client.utils.URIBuilder;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.util.EntityUtils;
import org.json.JSONObject;
import org.json.JSONArray;


public class TextAnalyzer {
    // please change this URI, if your subscription is in a different region!
    private static final String uriBase = "https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";
    
    private String subscriptionKey;

    public TextAnalyzer(String subscriptionKey) {
        this.subscriptionKey = subscriptionKey;
    }

    public double getSentimentAnalysis(String text) {
        double sentiment = 0;

        HttpClient httpclient = new DefaultHttpClient();
        
        try {
            URIBuilder builder = new URIBuilder(uriBase);

            // Prepare the URI for the REST API call.
            URI uri = builder.build();
            HttpPost request = new HttpPost(uri);

            // Request headers.
            request.setHeader("Content-Type", "application/json");
            request.setHeader("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request body.
            Documents documents = new Documents();
            documents.add("1", text);

            JSONObject requestBody = new JSONObject(documents);
            
            StringEntity reqEntity = new StringEntity(requestBody.toString());
            request.setEntity(reqEntity);

            // Execute the REST API call and get the response entity.
            HttpResponse response = httpclient.execute(request);
            HttpEntity entity = response.getEntity();

            if (entity != null) {
                // Format and display the JSON response.
                String jsonString = EntityUtils.toString(entity);
                JSONObject json = new JSONObject(jsonString);
                System.out.println("REST Response:\n");
                System.out.println(json.toString(2));

                JSONArray resultDocuments = json.getJSONArray("documents");
                if (resultDocuments.length() > 0) {
                    sentiment = resultDocuments.getJSONObject(0).getDouble("score");
                }

            }
        }
        catch (Exception e)
        {
            // Display error message.
            System.out.println(e.getMessage());
        }

        return sentiment;
    }
}