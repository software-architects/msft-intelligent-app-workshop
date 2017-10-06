public class App {

    public static final String subscriptionKey = "3090e06fc7b6442dabcb74f5b4fda4ca";
    
    public static void main( String[] args ) {
        testSentimentAnalysis("Good work, guys. Just wanted to say thank you!");
        testSentimentAnalysis("We have a question concerning a new product.");
        testSentimentAnalysis("Bad, bad, bad! The annoying save button still doesn't work.");
    }

    private static void testSentimentAnalysis(String text) {
        TextAnalyzer analyzer = new TextAnalyzer(subscriptionKey);
        double sentiment = analyzer.getSentimentAnalysis(text);

        System.out.println(text);
        System.out.println("Sentiment analysis: " + sentiment);        
    }
}