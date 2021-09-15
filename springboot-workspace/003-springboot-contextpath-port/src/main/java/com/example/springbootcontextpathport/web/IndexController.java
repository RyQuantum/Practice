package com.example.springbootcontextpathport.web;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

@Controller
public class IndexController {

    @RequestMapping(value = "/index")
    public @ResponseBody Object index() {
        return "Hello SpringBoot Web Project!";
    }
}
