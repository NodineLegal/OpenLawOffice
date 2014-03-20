--
-- PostgreSQL database dump
--

-- Dumped from database version 9.1.9
-- Dumped by pg_dump version 9.2.4
-- Started on 2014-03-16 22:46:42

SET statement_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 187 (class 3079 OID 11645)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2083 (class 0 OID 0)
-- Dependencies: 187
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 164 (class 1259 OID 98853)
-- Name: area; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE area (
    id integer NOT NULL,
    parent_id integer,
    name text NOT NULL,
    description text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.area OWNER TO postgres;

--
-- TOC entry 166 (class 1259 OID 98885)
-- Name: area_acl; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE area_acl (
    id integer NOT NULL,
    security_area_id integer NOT NULL,
    user_id integer NOT NULL,
    allow_flags integer NOT NULL,
    deny_flags integer NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.area_acl OWNER TO postgres;

--
-- TOC entry 165 (class 1259 OID 98883)
-- Name: area_acl_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE area_acl_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.area_acl_id_seq OWNER TO postgres;

--
-- TOC entry 2084 (class 0 OID 0)
-- Dependencies: 165
-- Name: area_acl_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE area_acl_id_seq OWNED BY area_acl.id;


--
-- TOC entry 163 (class 1259 OID 98851)
-- Name: area_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE area_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.area_id_seq OWNER TO postgres;

--
-- TOC entry 2085 (class 0 OID 0)
-- Dependencies: 163
-- Name: area_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE area_id_seq OWNED BY area.id;


--
-- TOC entry 176 (class 1259 OID 99092)
-- Name: contact; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE contact (
    id integer NOT NULL,
    is_organization boolean NOT NULL,
    nickname text,
    generation text,
    display_name_prefix text,
    surname text,
    middle_name text,
    given_name text,
    initials text,
    display_name text NOT NULL,
    email1_display_name text,
    email1_email_address text,
    email2_display_name text,
    email2_email_address text,
    email3_display_name text,
    email3_email_address text,
    fax1_display_name text,
    fax1_fax_number text,
    fax2_display_name text,
    fax2_fax_number text,
    fax3_display_name text,
    fax3_fax_number text,
    address1_display_name text,
    address1_address_street text,
    address1_address_city text,
    address1_address_state_or_province text,
    address1_address_postal_code text,
    address1_address_country text,
    address1_address_country_code text,
    address1_address_post_office_box text,
    address2_display_name text,
    address2_address_street text,
    address2_address_city text,
    address2_address_state_or_province text,
    address2_address_postal_code text,
    address2_address_country text,
    address2_address_country_code text,
    address2_address_post_office_box text,
    address3_display_name text,
    address3_address_street text,
    address3_address_city text,
    address3_address_state_or_province text,
    address3_address_postal_code text,
    address3_address_country text,
    address3_address_country_code text,
    address3_address_post_office_box text,
    telephone1_display_name text,
    telephone1_telephone_number text,
    telephone2_display_name text,
    telephone2_telephone_number text,
    telephone3_display_name text,
    telephone3_telephone_number text,
    telephone4_display_name text,
    telephone4_telephone_number text,
    telephone5_display_name text,
    telephone5_telephone_number text,
    telephone6_display_name text,
    telephone6_telephone_number text,
    telephone7_display_name text,
    telephone7_telephone_number text,
    telephone8_display_name text,
    telephone8_telephone_number text,
    telephone9_display_name text,
    telephone9_telephone_number text,
    telephone10_display_name text,
    telephone10_telephone_number text,
    birthday timestamp without time zone,
    wedding timestamp without time zone,
    title text,
    company_name text,
    department_name text,
    office_location text,
    manager_name text,
    assistant_name text,
    profession text,
    spouse_name text,
    language text,
    instant_messaging_address text,
    personal_home_page text,
    business_home_page text,
    gender text,
    referred_by_name text,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.contact OWNER TO postgres;

--
-- TOC entry 175 (class 1259 OID 99090)
-- Name: contact_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE contact_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.contact_id_seq OWNER TO postgres;

--
-- TOC entry 2086 (class 0 OID 0)
-- Dependencies: 175
-- Name: contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE contact_id_seq OWNED BY contact.id;


--
-- TOC entry 171 (class 1259 OID 98993)
-- Name: matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE matter (
    title text NOT NULL,
    parent_id uuid,
    synopsis text NOT NULL,
    id uuid NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.matter OWNER TO postgres;

--
-- TOC entry 178 (class 1259 OID 99128)
-- Name: matter_contact; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE matter_contact (
    id integer NOT NULL,
    matter_id uuid NOT NULL,
    contact_id integer NOT NULL,
    role text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.matter_contact OWNER TO postgres;

--
-- TOC entry 177 (class 1259 OID 99126)
-- Name: matter_contact_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE matter_contact_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.matter_contact_id_seq OWNER TO postgres;

--
-- TOC entry 2087 (class 0 OID 0)
-- Dependencies: 177
-- Name: matter_contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE matter_contact_id_seq OWNED BY matter_contact.id;


--
-- TOC entry 172 (class 1259 OID 99021)
-- Name: matter_tag; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE matter_tag (
    id uuid NOT NULL,
    matter_id uuid NOT NULL,
    tag_category_id integer,
    tag text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.matter_tag OWNER TO postgres;

--
-- TOC entry 174 (class 1259 OID 99056)
-- Name: responsible_user; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE responsible_user (
    id integer NOT NULL,
    matter_id uuid NOT NULL,
    user_id integer NOT NULL,
    responsibility text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.responsible_user OWNER TO postgres;

--
-- TOC entry 173 (class 1259 OID 99054)
-- Name: responsible_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE responsible_user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.responsible_user_id_seq OWNER TO postgres;

--
-- TOC entry 2088 (class 0 OID 0)
-- Dependencies: 173
-- Name: responsible_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE responsible_user_id_seq OWNED BY responsible_user.id;


--
-- TOC entry 167 (class 1259 OID 98916)
-- Name: secured_resource; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE secured_resource (
    id uuid NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.secured_resource OWNER TO postgres;

--
-- TOC entry 168 (class 1259 OID 98936)
-- Name: secured_resource_acl; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE secured_resource_acl (
    id uuid NOT NULL,
    secured_resource_id uuid NOT NULL,
    user_id integer NOT NULL,
    allow_flags integer NOT NULL,
    deny_flags integer NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.secured_resource_acl OWNER TO postgres;

--
-- TOC entry 170 (class 1259 OID 98968)
-- Name: tag_category; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE tag_category (
    id integer NOT NULL,
    name text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.tag_category OWNER TO postgres;

--
-- TOC entry 169 (class 1259 OID 98966)
-- Name: tag_category_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE tag_category_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tag_category_id_seq OWNER TO postgres;

--
-- TOC entry 2089 (class 0 OID 0)
-- Dependencies: 169
-- Name: tag_category_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE tag_category_id_seq OWNED BY tag_category.id;


--
-- TOC entry 181 (class 1259 OID 99214)
-- Name: task; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task (
    id bigint NOT NULL,
    title text NOT NULL,
    description text NOT NULL,
    projected_start timestamp without time zone,
    due_date timestamp without time zone,
    projected_end timestamp without time zone,
    actual_end timestamp without time zone,
    parent_id bigint,
    is_grouping_task bit(1) NOT NULL,
    sequential_predecessor_id bigint,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.task OWNER TO postgres;

--
-- TOC entry 182 (class 1259 OID 99248)
-- Name: task_assigned_contact; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_assigned_contact (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    contact_id integer NOT NULL,
    assignment_type smallint DEFAULT 1 NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.task_assigned_contact OWNER TO postgres;

--
-- TOC entry 180 (class 1259 OID 99212)
-- Name: task_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE task_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.task_id_seq OWNER TO postgres;

--
-- TOC entry 2090 (class 0 OID 0)
-- Dependencies: 180
-- Name: task_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE task_id_seq OWNED BY task.id;


--
-- TOC entry 183 (class 1259 OID 99304)
-- Name: task_matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_matter (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    matter_id uuid NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.task_matter OWNER TO postgres;

--
-- TOC entry 184 (class 1259 OID 99362)
-- Name: task_responsible_user; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_responsible_user (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    user_id integer NOT NULL,
    responsibility text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.task_responsible_user OWNER TO postgres;

--
-- TOC entry 185 (class 1259 OID 99403)
-- Name: task_tag; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_tag (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    tag_category_id integer,
    tag text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.task_tag OWNER TO postgres;

--
-- TOC entry 186 (class 1259 OID 99461)
-- Name: task_time; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_time (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    time_id uuid NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.task_time OWNER TO postgres;

--
-- TOC entry 179 (class 1259 OID 99187)
-- Name: time; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "time" (
    id uuid NOT NULL,
    start timestamp without time zone NOT NULL,
    stop timestamp without time zone NOT NULL,
    worker_contact_id integer NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public."time" OWNER TO postgres;

--
-- TOC entry 162 (class 1259 OID 98840)
-- Name: user; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "user" (
    id integer NOT NULL,
    username text NOT NULL,
    password text NOT NULL,
    password_salt text NOT NULL,
    user_auth_token uuid,
    user_auth_token_expiry timestamp without time zone,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public."user" OWNER TO postgres;

--
-- TOC entry 161 (class 1259 OID 98838)
-- Name: user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.user_id_seq OWNER TO postgres;

--
-- TOC entry 2091 (class 0 OID 0)
-- Dependencies: 161
-- Name: user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE user_id_seq OWNED BY "user".id;


--
-- TOC entry 1946 (class 2604 OID 98856)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area ALTER COLUMN id SET DEFAULT nextval('area_id_seq'::regclass);


--
-- TOC entry 1947 (class 2604 OID 98888)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl ALTER COLUMN id SET DEFAULT nextval('area_acl_id_seq'::regclass);


--
-- TOC entry 1950 (class 2604 OID 99095)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact ALTER COLUMN id SET DEFAULT nextval('contact_id_seq'::regclass);


--
-- TOC entry 1951 (class 2604 OID 99131)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact ALTER COLUMN id SET DEFAULT nextval('matter_contact_id_seq'::regclass);


--
-- TOC entry 1949 (class 2604 OID 99059)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user ALTER COLUMN id SET DEFAULT nextval('responsible_user_id_seq'::regclass);


--
-- TOC entry 1948 (class 2604 OID 98971)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_category ALTER COLUMN id SET DEFAULT nextval('tag_category_id_seq'::regclass);


--
-- TOC entry 1952 (class 2604 OID 99217)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task ALTER COLUMN id SET DEFAULT nextval('task_id_seq'::regclass);


--
-- TOC entry 1945 (class 2604 OID 98843)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "user" ALTER COLUMN id SET DEFAULT nextval('user_id_seq'::regclass);


--
-- TOC entry 1962 (class 2606 OID 99517)
-- Name: UQ_area_acl_Area_User; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "UQ_area_acl_Area_User" UNIQUE (security_area_id, user_id);


--
-- TOC entry 1968 (class 2606 OID 99544)
-- Name: UQ_secured_resource_acl_SecuredResource_User; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "UQ_secured_resource_acl_SecuredResource_User" UNIQUE (secured_resource_id, user_id);


--
-- TOC entry 1991 (class 2606 OID 99571)
-- Name: UQ_task_matter_Task_Matter; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "UQ_task_matter_Task_Matter" UNIQUE (task_id, matter_id);


--
-- TOC entry 1964 (class 2606 OID 98890)
-- Name: area_acl_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT area_acl_pkey PRIMARY KEY (id);


--
-- TOC entry 1959 (class 2606 OID 98861)
-- Name: area_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY area
    ADD CONSTRAINT area_pkey PRIMARY KEY (id);


--
-- TOC entry 1981 (class 2606 OID 99100)
-- Name: contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT contact_pkey PRIMARY KEY (id);


--
-- TOC entry 1983 (class 2606 OID 99136)
-- Name: matter_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT matter_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 1975 (class 2606 OID 99000)
-- Name: matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT matter_pkey PRIMARY KEY (id);


--
-- TOC entry 1977 (class 2606 OID 99028)
-- Name: matter_tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT matter_tag_pkey PRIMARY KEY (id);


--
-- TOC entry 1979 (class 2606 OID 99064)
-- Name: responsible_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT responsible_user_pkey PRIMARY KEY (id);


--
-- TOC entry 1970 (class 2606 OID 98940)
-- Name: secured_resource_acl_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT secured_resource_acl_pkey PRIMARY KEY (id);


--
-- TOC entry 1966 (class 2606 OID 98920)
-- Name: secured_resource_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY secured_resource
    ADD CONSTRAINT secured_resource_pkey PRIMARY KEY (id);


--
-- TOC entry 1972 (class 2606 OID 98976)
-- Name: tag_category_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY tag_category
    ADD CONSTRAINT tag_category_pkey PRIMARY KEY (id);


--
-- TOC entry 1989 (class 2606 OID 99253)
-- Name: task_assigned_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT task_assigned_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 1993 (class 2606 OID 99308)
-- Name: task_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT task_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 1987 (class 2606 OID 99222)
-- Name: task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task
    ADD CONSTRAINT task_pkey PRIMARY KEY (id);


--
-- TOC entry 1995 (class 2606 OID 99369)
-- Name: task_responsible_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT task_responsible_user_pkey PRIMARY KEY (id);


--
-- TOC entry 1997 (class 2606 OID 99410)
-- Name: task_tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT task_tag_pkey PRIMARY KEY (id);


--
-- TOC entry 1999 (class 2606 OID 99465)
-- Name: task_time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT task_time_pkey PRIMARY KEY (id);


--
-- TOC entry 1985 (class 2606 OID 99191)
-- Name: time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT time_pkey PRIMARY KEY (id);


--
-- TOC entry 1957 (class 2606 OID 98848)
-- Name: user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "user"
    ADD CONSTRAINT user_pkey PRIMARY KEY (id);


--
-- TOC entry 1960 (class 1259 OID 98882)
-- Name: uidx_area_name; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX uidx_area_name ON area USING btree (name);


--
-- TOC entry 1973 (class 1259 OID 98992)
-- Name: uidx_tagcategory_name; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX uidx_tagcategory_name ON tag_category USING btree (name);


--
-- TOC entry 1954 (class 1259 OID 98850)
-- Name: uidx_user_userauthtoken; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX uidx_user_userauthtoken ON "user" USING btree (user_auth_token);


--
-- TOC entry 1955 (class 1259 OID 98849)
-- Name: uidx_user_username; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX uidx_user_username ON "user" USING btree (username);


--
-- TOC entry 2004 (class 2606 OID 99491)
-- Name: FK_area_acl_area_SecurityAreaId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_area_SecurityAreaId" FOREIGN KEY (security_area_id) REFERENCES area(id);


--
-- TOC entry 2005 (class 2606 OID 99496)
-- Name: FK_area_acl_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2006 (class 2606 OID 99501)
-- Name: FK_area_acl_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2007 (class 2606 OID 99506)
-- Name: FK_area_acl_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2008 (class 2606 OID 99511)
-- Name: FK_area_acl_user_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_user_UserId" FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2000 (class 2606 OID 98862)
-- Name: FK_area_area_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area
    ADD CONSTRAINT "FK_area_area_ParentId" FOREIGN KEY (parent_id) REFERENCES area(id);


--
-- TOC entry 2001 (class 2606 OID 98867)
-- Name: FK_area_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area
    ADD CONSTRAINT "FK_area_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2003 (class 2606 OID 98877)
-- Name: FK_area_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area
    ADD CONSTRAINT "FK_area_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2002 (class 2606 OID 98872)
-- Name: FK_area_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area
    ADD CONSTRAINT "FK_area_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2034 (class 2606 OID 99101)
-- Name: FK_contact_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT "FK_contact_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2036 (class 2606 OID 99111)
-- Name: FK_contact_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT "FK_contact_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2035 (class 2606 OID 99106)
-- Name: FK_contact_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT "FK_contact_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2037 (class 2606 OID 99162)
-- Name: FK_matter_contact_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2038 (class 2606 OID 99167)
-- Name: FK_matter_contact_user_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2039 (class 2606 OID 99172)
-- Name: FK_matter_contact_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2041 (class 2606 OID 99182)
-- Name: FK_matter_contact_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2040 (class 2606 OID 99177)
-- Name: FK_matter_contact_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2020 (class 2606 OID 99001)
-- Name: FK_matter_matter_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_matter_ParentId" FOREIGN KEY (parent_id) REFERENCES matter(id);


--
-- TOC entry 2024 (class 2606 OID 99029)
-- Name: FK_matter_tag_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2025 (class 2606 OID 99034)
-- Name: FK_matter_tag_tag_category_TagCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_tag_category_TagCategoryId" FOREIGN KEY (tag_category_id) REFERENCES tag_category(id);


--
-- TOC entry 2026 (class 2606 OID 99039)
-- Name: FK_matter_tag_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2028 (class 2606 OID 99049)
-- Name: FK_matter_tag_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2027 (class 2606 OID 99044)
-- Name: FK_matter_tag_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2021 (class 2606 OID 99006)
-- Name: FK_matter_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2023 (class 2606 OID 99016)
-- Name: FK_matter_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2022 (class 2606 OID 99011)
-- Name: FK_matter_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2029 (class 2606 OID 99065)
-- Name: FK_responsible_user_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2031 (class 2606 OID 99075)
-- Name: FK_responsible_user_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2033 (class 2606 OID 99085)
-- Name: FK_responsible_user_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2032 (class 2606 OID 99080)
-- Name: FK_responsible_user_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2030 (class 2606 OID 99070)
-- Name: FK_responsible_user_user_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_user_UserId" FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2012 (class 2606 OID 99518)
-- Name: FK_secured_resource_acl_secured_resource_SecuredResourceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_secured_resource_SecuredResourceId" FOREIGN KEY (secured_resource_id) REFERENCES secured_resource(id);


--
-- TOC entry 2013 (class 2606 OID 99523)
-- Name: FK_secured_resource_acl_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2014 (class 2606 OID 99528)
-- Name: FK_secured_resource_acl_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2015 (class 2606 OID 99533)
-- Name: FK_secured_resource_acl_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2016 (class 2606 OID 99538)
-- Name: FK_secured_resource_acl_user_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_user_UserId" FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2009 (class 2606 OID 98921)
-- Name: FK_secured_resource_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource
    ADD CONSTRAINT "FK_secured_resource_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2011 (class 2606 OID 98931)
-- Name: FK_secured_resource_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource
    ADD CONSTRAINT "FK_secured_resource_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2010 (class 2606 OID 98926)
-- Name: FK_secured_resource_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource
    ADD CONSTRAINT "FK_secured_resource_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2017 (class 2606 OID 98977)
-- Name: FK_tag_category_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_category
    ADD CONSTRAINT "FK_tag_category_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2019 (class 2606 OID 98987)
-- Name: FK_tag_category_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_category
    ADD CONSTRAINT "FK_tag_category_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2018 (class 2606 OID 98982)
-- Name: FK_tag_category_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_category
    ADD CONSTRAINT "FK_tag_category_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2055 (class 2606 OID 99274)
-- Name: FK_task_assigned_contact_contact_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_contact_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2054 (class 2606 OID 99269)
-- Name: FK_task_assigned_contact_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2051 (class 2606 OID 99254)
-- Name: FK_task_assigned_contact_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2052 (class 2606 OID 99259)
-- Name: FK_task_assigned_contact_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2053 (class 2606 OID 99264)
-- Name: FK_task_assigned_contact_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2056 (class 2606 OID 99545)
-- Name: FK_task_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2057 (class 2606 OID 99550)
-- Name: FK_task_matter_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2058 (class 2606 OID 99555)
-- Name: FK_task_matter_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2059 (class 2606 OID 99560)
-- Name: FK_task_matter_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2060 (class 2606 OID 99565)
-- Name: FK_task_matter_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2064 (class 2606 OID 99385)
-- Name: FK_task_responsible_user_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2061 (class 2606 OID 99370)
-- Name: FK_task_responsible_user_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2062 (class 2606 OID 99375)
-- Name: FK_task_responsible_user_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2065 (class 2606 OID 99390)
-- Name: FK_task_responsible_user_user_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_user_MatterId" FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2063 (class 2606 OID 99380)
-- Name: FK_task_responsible_user_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2067 (class 2606 OID 99416)
-- Name: FK_task_tag_tag_category_TagCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_tag_category_TagCategoryId" FOREIGN KEY (tag_category_id) REFERENCES tag_category(id);


--
-- TOC entry 2066 (class 2606 OID 99411)
-- Name: FK_task_tag_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2068 (class 2606 OID 99421)
-- Name: FK_task_tag_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2069 (class 2606 OID 99426)
-- Name: FK_task_tag_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2070 (class 2606 OID 99431)
-- Name: FK_task_tag_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2049 (class 2606 OID 99294)
-- Name: FK_task_task_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_task_ParentId" FOREIGN KEY (parent_id) REFERENCES task(id);


--
-- TOC entry 2050 (class 2606 OID 99299)
-- Name: FK_task_task_SequentialPredecessorId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_task_SequentialPredecessorId" FOREIGN KEY (sequential_predecessor_id) REFERENCES task(id);


--
-- TOC entry 2074 (class 2606 OID 99481)
-- Name: FK_task_time_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2071 (class 2606 OID 99466)
-- Name: FK_task_time_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2072 (class 2606 OID 99471)
-- Name: FK_task_time_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2073 (class 2606 OID 99476)
-- Name: FK_task_time_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2075 (class 2606 OID 99486)
-- Name: FK_task_time_user_TimeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_TimeId" FOREIGN KEY (time_id) REFERENCES "time"(id);


--
-- TOC entry 2046 (class 2606 OID 99279)
-- Name: FK_task_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2047 (class 2606 OID 99284)
-- Name: FK_task_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2048 (class 2606 OID 99289)
-- Name: FK_task_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2042 (class 2606 OID 99192)
-- Name: FK_time_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2043 (class 2606 OID 99197)
-- Name: FK_time_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2044 (class 2606 OID 99202)
-- Name: FK_time_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2045 (class 2606 OID 99207)
-- Name: FK_time_user_WorkerContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_WorkerContactId" FOREIGN KEY (worker_contact_id) REFERENCES contact(id);


--
-- TOC entry 2082 (class 0 OID 0)
-- Dependencies: 5
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2014-03-16 22:46:43

--
-- PostgreSQL database dump complete
--

