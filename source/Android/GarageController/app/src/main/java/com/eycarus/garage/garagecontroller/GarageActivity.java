package com.eycarus.garage.garagecontroller;

import android.app.Activity;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;

import com.rabbitmq.client.Channel;
import com.rabbitmq.client.Connection;
import com.rabbitmq.client.ConnectionFactory;


public class GarageActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_garage);

        //StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        //StrictMode.setThreadPolicy(policy);

        final Button buttonLeft = (Button) findViewById(R.id.button_left);
        buttonLeft.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                new Worker(1).execute();
            }
        });

        final Button buttonRight = (Button) findViewById(R.id.button_right);
        buttonRight.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                new Worker(2).execute();
            }
        });
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.garage, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();
        if (id == R.id.action_settings) {
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    private class Worker extends AsyncTask {

        private int _door =0;
        public Worker(int door) {
            _door = door;
        }

        @Override
        protected Object doInBackground(Object... arg0) {
            sendButtonCommand(_door);
            return null;
        }
    }

    private void connect() {
    }

    private void sendButtonCommand(int buttonNumber) {
        ConnectionFactory factory = new ConnectionFactory();
        factory.setHost("rv-broker.cloudapp.net");
        factory.setUsername("tester");
        factory.setPassword("GoGoTester");
        factory.setPort(5672);
        //factory.setRequestedHeartbeat(30);


        try {
            Connection connection = factory.newConnection();
            Channel channel = connection.createChannel();
            String message = "Door " + buttonNumber;
            channel.basicPublish("", "GarageKorvettveien7",null,message.getBytes());
            channel.close();
            connection.close();
        } catch (Exception e) {
            e.printStackTrace();
        }




        //TODO: Contruct Command and send
    }

}
